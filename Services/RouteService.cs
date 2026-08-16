using Gamana_Muttopalvelu_Backend.DTO;
using System.Globalization;
using System.Text.Json;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IRouteService
    {
        Task<RouteResultDto?> CalculateBestRouteAsync(CalculateRouteRequest request);
        Task EnsureCoordinatesAsync(AddressDto address);
    }

    public class RouteService : IRouteService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public RouteService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<RouteResultDto?> CalculateBestRouteAsync(CalculateRouteRequest request)
        {
            // 1. Geocode missing coordinates
            await EnsureCoordinatesAsync(request.Office);
            foreach (var pickup in request.Pickups)
            {
                await EnsureCoordinatesAsync(pickup);
            }
            foreach (var drop in request.Drops)
            {
                await EnsureCoordinatesAsync(drop);
            }

            // 2. Sort Pickups starting from Office (TSP)
            var sortedPickups = OptimizeWaypointsOrder(request.Office, request.Pickups);

            // 3. Sort Drops starting from the last pickup point (TSP)
            var lastPickup = sortedPickups.LastOrDefault() ?? request.Office;
            var sortedDrops = OptimizeWaypointsOrder(lastPickup, request.Drops);

            // 4. Build complete journey sequence: Office -> Pickups -> Drops
            var finalPath = new List<AddressDto> { request.Office };
            finalPath.AddRange(sortedPickups);
            finalPath.AddRange(sortedDrops);

            // Validate that coordinates exist before calling OSRM
            if (finalPath.Any(p => p.Latitude == 0 || p.Longitude == 0))
            {
                Console.WriteLine("[Route Error] One or more address waypoints could not be geocoded.");
                return null;
            }

            // 5. Build coordinates string for OSRM API (Format: Longitude,Latitude)
            // 5. Build coordinates string for OSRM API (Format: Longitude,Latitude)
            var coordinates = string.Join(";", finalPath.Select(w =>
                $"{w.Longitude.ToString(CultureInfo.InvariantCulture)},{w.Latitude.ToString(CultureInfo.InvariantCulture)}"));

            string osrmBaseUrl = _config["OsrmSettings:BaseUrl"] ?? "https://router.project-osrm.org";
            var requestUrl = $"{osrmBaseUrl}/route/v1/driving/{coordinates}?overview=full&geometries=geojson";

            // Construct request explicitly to attach headers
            var osrmRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            osrmRequest.Headers.Add("User-Agent", "GamanaMuuttopalveluBackend/1.0 (contact@gamana.fi)");

            var response = await _httpClient.SendAsync(osrmRequest);
            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[OSRM Error] Status: {response.StatusCode}, Details: {errorText}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("routes", out var routes) || routes.GetArrayLength() == 0)
            {
                Console.WriteLine("[OSRM Error] No routes found between provided waypoints.");
                return null;
            }

            var route = routes[0];
            double distanceMeters = route.GetProperty("distance").GetDouble();
            double durationSeconds = route.GetProperty("duration").GetDouble();

            return new RouteResultDto
            {
                TotalDistanceKm = Math.Round(distanceMeters / 1000.0, 2),
                TotalDurationMinutes = Math.Round(durationSeconds / 60.0, 1),
                EncodedPolyline = route.GetProperty("geometry").ToString(), // Raw GeoJSON geometry
                OptimizedWaypoints = finalPath
            };
        }

        #region Geocoding Execution Logic

        public async Task EnsureCoordinatesAsync(AddressDto address)
        {
            if (address.Latitude != 0 && address.Longitude != 0) return;

            string apiKey = _config["Digitransit:ApiKey"] ?? "";
            string queryText = $"{address.Street} {address.HouseNumber}, {address.PostalCode} {address.City}".Trim();

            if (string.IsNullOrWhiteSpace(queryText) || queryText == ",") return;

            bool hasValidKey = !string.IsNullOrWhiteSpace(apiKey) &&
                               !apiKey.Contains("PLACEHOLDER", StringComparison.OrdinalIgnoreCase);

            // 1. Primary Attempt: Digitransit Geocoding (Pelias)
            if (hasValidKey)
            {
                var digitransitUrl = $"https://api.digitransit.fi/geocoding/v1/search?text={Uri.EscapeDataString(queryText)}&size=1&digitransit-subscription-key={apiKey}";
                var request = new HttpRequestMessage(HttpMethod.Get, digitransitUrl);

                request.Headers.Add("User-Agent", "GamanaMuuttopalveluBackend/1.0 (contact@gamana.fi)");
                request.Headers.Add("digitransit-subscription-key", apiKey);

                try
                {
                    var response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(json);
                        var root = doc.RootElement;

                        if (root.TryGetProperty("features", out var features) && features.GetArrayLength() > 0)
                        {
                            // GeoJSON geometry coordinates are [Longitude, Latitude]
                            var coords = features[0].GetProperty("geometry").GetProperty("coordinates");
                            address.Longitude = coords[0].GetDouble();
                            address.Latitude = coords[1].GetDouble();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Digitransit Exception: {ex.Message}");
                }
            }

            // 2. Fallback Attempt: OpenStreetMap Nominatim
            try
            {
                var nominatimUrl = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(queryText)}&format=json&limit=1";
                var request = new HttpRequestMessage(HttpMethod.Get, nominatimUrl);
                request.Headers.Add("User-Agent", "GamanaMuuttopalveluBackend/1.0 (contact@gamana.fi)");

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);

                    if (doc.RootElement.GetArrayLength() > 0)
                    {
                        var first = doc.RootElement[0];
                        address.Latitude = double.Parse(first.GetProperty("lat").GetString()!, CultureInfo.InvariantCulture);
                        address.Longitude = double.Parse(first.GetProperty("lon").GetString()!, CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nominatim Exception: {ex.Message}");
            }
        }

        #endregion

        #region Route Optimization Logic (TSP)

        private List<AddressDto> OptimizeWaypointsOrder(AddressDto startPoint, List<AddressDto> points)
        {
            if (points == null || points.Count <= 1)
                return points ?? new List<AddressDto>();

            var permutations = GetPermutations(points, points.Count);
            List<AddressDto> bestOrder = points;
            double minDistance = double.MaxValue;

            foreach (var perm in permutations)
            {
                var currentOrder = perm.ToList();
                double currentDistance = CalculatePathDistance(startPoint, currentOrder);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    bestOrder = currentOrder;
                }
            }

            return bestOrder;
        }

        private double CalculatePathDistance(AddressDto start, List<AddressDto> stops)
        {
            double total = 0;
            var current = start;

            foreach (var next in stops)
            {
                total += HaversineDistance(current.Latitude, current.Longitude, next.Latitude, next.Longitude);
                current = next;
            }

            return total;
        }

        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0;
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            return EarthRadiusKm * (2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
        }

        private static double ToRadians(double angle) => (Math.PI / 180.0) * angle;

        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        #endregion
    }
}