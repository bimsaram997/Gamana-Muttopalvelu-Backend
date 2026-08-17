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
        private readonly IEmailQueue _emailQueue; 

        public BookingService(
            IUserRepository userRepository,
            IBookingRepository bookingRepository,
            IAddressRepository addressRepository,
            IEmailService emailService,
            IRouteService routeService,
            IEmailQueue emailQueue)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _addressRepository = addressRepository;
            _emailService = emailService;
            _routeService = routeService;
            _emailQueue = emailQueue;
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {
            // 0. Calculate Optimal Driving Route from HQ Office -> Pickups -> Dropoff
            var dropoffs = dto.DropoffLocation != null
                ? new List<AddressDto> { dto.DropoffLocation }
                : new List<AddressDto>();

          
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
            // 4. Queue the email job (Non-blocking background execution)
            _emailQueue.QueueEmail(dto, booking.Id);


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
                
            };
        }

        public async Task<BookingDetailResponseDto?> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null) return null;

            var pickupLocations = booking.Addresses
                .Where(a => a.Type == AddressType.Pickup)
                .Select(MapToAddressDto)
                .ToList();

            var dropoffAddress = booking.Addresses
                .FirstOrDefault(a => a.Type == AddressType.Dropoff);

            var dropoffLocation = dropoffAddress != null ? MapToAddressDto(dropoffAddress) : null;

            // Calculate Route for the GetById Details
            RouteResultDto? routeResult = null;
            try
            {
                var routeRequest = new CalculateRouteRequest
                {
                    Office = _officeAddress,
                    Pickups = pickupLocations,
                    Drops = dropoffLocation != null ? new List<AddressDto> { dropoffLocation } : new List<AddressDto>()
                };

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                routeResult = await _routeService.CalculateBestRouteAsync(routeRequest);
            }
            catch (Exception ex)
            {
                //_logger.LogWarning(ex, "Failed to fetch route for Booking ID: {BookingId}", bookingId);
            }

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

                // Address & Route Information
                PickupLocations = pickupLocations,
                DropoffLocation = dropoffLocation,
                routeResultDto = routeResult
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
