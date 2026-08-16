using Gamana_Muttopalvelu_Backend.Data;
using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Enums;
using Gamana_Muttopalvelu_Backend.Repositories;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto);
        Task<BookingDetailResponseDto> GetBookingByIdAsync(Guid bookingId);
    }
    public class BookingService : IBookingService
    {
        private readonly AddressDto _officeAddress = new AddressDto
        {
            Label = "Ali-Huikkaantie 4 A-B, Tampere",
            Street = "Ali-Huikkaantie",
            HouseNumber = "4 A-B",
            PostalCode = "33560",
            City = "Tampere",
            Latitude = 61.494597,
            Longitude = 23.839757,
            Floor = 1,
            HasElevator = true
        };
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IEmailService _emailService;
        private readonly IRouteService _routeService;

        public BookingService(
            IUserRepository userRepository,
            IBookingRepository bookingRepository,
            IAddressRepository addressRepository,
            IEmailService emailService,
            IRouteService routeService
            )
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _addressRepository = addressRepository;
            _emailService = emailService;
            _routeService = routeService;
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {
            // 0. Calculate Optimal Driving Route from HQ Office -> Pickups -> Dropoff
            var dropoffs = dto.DropoffLocation != null
                ? new List<AddressDto> { dto.DropoffLocation }
                : new List<AddressDto>();

            var routeRequest = new CalculateRouteRequest
            {
                Office = _officeAddress,
                Pickups = dto.PickupLocations,
                Drops = dropoffs
            };

            var routeResult = await _routeService.CalculateBestRouteAsync(routeRequest);

            // Reassign optimized pickup locations list back to DTO
            if (routeResult?.OptimizedWaypoints != null && routeResult.OptimizedWaypoints.Count > 0)
            {
                var sortedPickups = routeResult.OptimizedWaypoints
                    .Where(w => dto.PickupLocations.Any(p => p.Street == w.Street && p.HouseNumber == w.HouseNumber))
                    .ToList();

                if (sortedPickups.Any())
                {
                    dto.PickupLocations = sortedPickups;
                }
            }
            // 1. Handle User via UserRepository
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = dto.FullName,
                    Email = dto.Email.Trim().ToLower(),
                    Phone = dto.Phone,
                    CreatedAt = DateTime.UtcNow
                };
                await _userRepository.AddAsync(user);
            }
            else
            {
                user.FullName = dto.FullName;
                user.Phone = dto.Phone;
            }

            // 2. Prepare Addresses
            var addresses = new List<Address>();

            foreach (var p in dto.PickupLocations)
            {
                addresses.Add(MapToAddressEntity(p, AddressType.Pickup));
            }

            if (dto.DropoffLocation != null)
            {
                addresses.Add(MapToAddressEntity(dto.DropoffLocation, AddressType.Dropoff));
            }

            // Add via AddressRepository
            await _addressRepository.AddRangeAsync(addresses);

            // 3. Create Booking via BookingRepository
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                User = user,
                SelectedPackageId = dto.SelectedPackageId,
                EstimatedHours = dto.EstimatedHours,
                IncludeCleaning = dto.IncludeCleaning,
                Addresses = addresses,
                Notes = dto.Notes,
                ServiceDate = dto.ServiceDate,
                TotalPrice = dto.TotalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);

            // Single transaction save point
            await _bookingRepository.SaveChangesAsync();
            try
            {
                await _emailService.SendAdminNewBookingEmailAsync(dto, booking.Id);
            }
            catch (Exception ex)
            {
                // Log error so DB save isn't rolled back if SMTP fails
                Console.WriteLine($"Email failed: {ex.Message}");
            }

            return new BookingResponseDto
            {
                BookingId = booking.Id,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TotalAddresses = booking.Addresses.Count,
                ServiceDate = booking.ServiceDate,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                routeResultDto = routeResult
            };
        }

        public async Task<BookingDetailResponseDto?> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return null;
            }

            // Map Pickup and Dropoff addresses based on AddressType enum
            var pickupLocations = booking.Addresses
                .Where(a => a.Type == AddressType.Pickup)
                .Select(MapToAddressDto)
                .ToList();

            var dropoffAddress = booking.Addresses
                .FirstOrDefault(a => a.Type == AddressType.Dropoff);

            return new BookingDetailResponseDto
            {
                BookingId = booking.Id,
                SelectedPackageId = booking.SelectedPackageId,
                EstimatedHours = booking.EstimatedHours,
                IncludeCleaning = booking.IncludeCleaning,
                Notes = booking.Notes,
                ServiceDate = booking.ServiceDate,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,

                // User Information
                UserId = booking.UserId,
                FullName = booking.User?.FullName ?? string.Empty,
                Email = booking.User?.Email ?? string.Empty,
                Phone = booking.User?.Phone ?? string.Empty,

                // Address Information
                PickupLocations = pickupLocations,
                DropoffLocation = dropoffAddress != null ? MapToAddressDto(dropoffAddress) : null
            };
        }

        private static AddressDto MapToAddressDto(Address address)
        {
            return new AddressDto
            {
                Label = address.Label,
                Street = address.Street,
                HouseNumber = address.HouseNumber,
                PostalCode = address.PostalCode,
                City = address.City,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Floor = address.Floor,
                HasElevator = address.HasElevator
            };
        }

        private static Address MapToAddressEntity(AddressDto dto, AddressType type)
        {
            return new Address
            {
                Id = Guid.NewGuid(),
                Type = type,
                Label = dto.Label,
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                PostalCode = dto.PostalCode,
                City = dto.City,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Floor = dto.Floor,
                HasElevator = dto.HasElevator
            };
        }
    }
}
