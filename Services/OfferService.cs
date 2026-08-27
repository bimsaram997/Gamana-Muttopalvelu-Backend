using Gamana_Muttopalvelu_Backend.Data;
using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Enums;
using Gamana_Muttopalvelu_Backend.Repositories;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IOfferService
    {
        Task<OfferResponseDto> CreateOfferAsync(CreateOfferDto dto);
        Task<OfferDetailResponseDto?> GetOfferByIdAsync(Guid offerId);
    }

    public class OfferService : IOfferService
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
        private readonly IOfferRepository _offerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IEmailService _emailService;
        private readonly IRouteService _routeService;
        private readonly IEmailQueue _emailQueue;
        private readonly IRequestedServiceRepository _requestedServiceRepository;

        public OfferService(
            IUserRepository userRepository,
            IOfferRepository offerRepository,
            IAddressRepository addressRepository,
            IEmailService emailService,
            IRouteService routeService,
            IRequestedServiceRepository requestedServiceRepository,
            IEmailQueue emailQueue)
        {
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _addressRepository = addressRepository;
            _emailService = emailService;
            _requestedServiceRepository = requestedServiceRepository;
            _routeService = routeService;
            _emailQueue = emailQueue;
        }

        public async Task<OfferResponseDto> CreateOfferAsync(CreateOfferDto dto)
        {
            // 1. Handle User
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

            if (dto.DepartureAddress != null)
            {
                addresses.Add(MapToAddressEntity(dto.DepartureAddress, AddressType.Pickup));
            }

            if (dto.DestinationAddress != null)
            {
                addresses.Add(MapToAddressEntity(dto.DestinationAddress, AddressType.Dropoff));
            }

            await _addressRepository.AddRangeAsync(addresses);

            // 3. Create Offer Entity
            var offerId = Guid.NewGuid();

            var offer = new Offer
            {
                Id = offerId,
                UserId = user.Id,
                DesiredMovingDate = dto.DesiredMovingDate,
                Addresses = addresses,
                Phone = dto.Phone,
                Email = dto.Email,
                AdditionalInfo = dto.AdditionalInfo,
                PrivacyAgreed = dto.PrivacyAgreed,
                CreatedAt = DateTime.UtcNow,
                RequestedServices = dto.ServiceIds.Select(serviceId => new RequestedService
                {
                    Id = Guid.NewGuid(),
                    OfferId = offerId,
                    ServiceId = serviceId
                }).ToList()
            };

            // Add top-level aggregate entity (EF Core automatically manages child entities)
            await _offerRepository.AddAsync(offer);

            // Save transaction changes
            await _offerRepository.SaveChangesAsync();

            // 4. Queue background notification
            _emailQueue.QueueEmail(EmailType.Offer, dto, offer.Id);

            return new OfferResponseDto
            {
                OfferId = offer.Id,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TotalAddresses = offer.Addresses.Count,
                DesiredMovingDate = offer.DesiredMovingDate,
                CreatedAt = offer.CreatedAt,
                AdditionalInfo = offer.AdditionalInfo,
                Phone = offer.Phone
            };
        }

        public async Task<OfferDetailResponseDto?> GetOfferByIdAsync(Guid offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) return null;

            var departureAddress = offer.Addresses
                .FirstOrDefault(a => a.Type == AddressType.Pickup);

            var destinationAddress = offer.Addresses
                .FirstOrDefault(a => a.Type == AddressType.Dropoff);

            var departureDto = departureAddress != null ? MapToAddressDto(departureAddress) : null;
            var destinationDto = destinationAddress != null ? MapToAddressDto(destinationAddress) : null;

            // Route Calculation
            RouteResultDto? routeResult = null;
            try
            {
                var routeRequest = new CalculateRouteRequest
                {
                    Office = _officeAddress,
                    Pickups = departureDto != null ? new List<AddressDto> { departureDto } : new List<AddressDto>(),
                    Drops = destinationDto != null ? new List<AddressDto> { destinationDto } : new List<AddressDto>()
                };

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                routeResult = await _routeService.CalculateBestRouteAsync(routeRequest);
            }
            catch
            {
                // Soft fail on route calculation
            }

            return new OfferDetailResponseDto
            {
                OfferId = offer.Id,
                TotalAddresses = offer.Addresses.Count,
                DesiredMovingDate = offer.DesiredMovingDate,
                CreatedAt = offer.CreatedAt,
                AdditionalInfo = offer.AdditionalInfo,
                PrivacyAgreed = offer.PrivacyAgreed,
                UserId = offer.UserId,
                FullName = offer.User?.FullName ?? string.Empty,
                Email = offer.User?.Email ?? string.Empty,
                Phone = offer.User?.Phone ?? string.Empty,
                DepartureAddress = departureDto,
                DestinationAddress = destinationDto,
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