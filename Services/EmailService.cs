using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IEmailService
    {
        Task SendAdminNewBookingEmailAsync(CreateBookingDto dto, Guid bookingId);
        Task SendAdminNewOfferEmailAsync(CreateOfferDto dto, Guid offerId);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAdminNewBookingEmailAsync(CreateBookingDto dto, Guid bookingId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress("Gamana Admin", _settings.AdminEmail));
            message.Subject = $"New Moving Booking #{bookingId.ToString()[..8]} - {dto.FullName}";

            var pickupAddresses = string.Join("<br/>", dto.PickupLocations.Select(p => $"• <strong>{p.Label}</strong> (Floor: {p.Floor}, Elevator: {(p.HasElevator ? "Yes" : "No")})"));
            var dropoffAddress = dto.DropoffLocation != null
                ? $"<strong>{dto.DropoffLocation.Label}</strong> (Floor: {dto.DropoffLocation.Floor}, Elevator: {(dto.DropoffLocation.HasElevator ? "Yes" : "No")})"
                : "N/A";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px;'>
                        <h2 style='color: #dc3545; margin-top: 0;'>🚨 New Booking Received!</h2>
                        <p style='font-size: 14px; color: #666;'><strong>Booking ID:</strong> {bookingId}</p>
                        
                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        
                        <h3 style='color: #222; margin-bottom: 10px;'>Customer Details</h3>
                        <p style='margin: 4px 0;'><strong>Name:</strong> {dto.FullName}</p>
                        <p style='margin: 4px 0;'><strong>Email:</strong> <a href='mailto:{dto.Email}' style='color: #0056b3;'>{dto.Email}</a></p>
                        <p style='margin: 4px 0;'><strong>Phone:</strong> <a href='tel:{dto.Phone}' style='color: #0056b3;'>{dto.Phone}</a></p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        
                        <h3 style='color: #222; margin-bottom: 10px;'>Service Details</h3>
                        <p style='margin: 4px 0;'><strong>Service Date:</strong> {dto.ServiceDate:g}</p>
                        <p style='margin: 4px 0;'><strong>Package ID:</strong> {dto.SelectedPackageId}</p>
                        <p style='margin: 4px 0;'><strong>Estimated Hours:</strong> {dto.EstimatedHours} hrs</p>
                        <p style='margin: 4px 0;'><strong>Move-Out Cleaning:</strong> {(dto.IncludeCleaning ? "Yes (+110€)" : "No")}</p>
                        <p style='margin: 8px 0; font-size: 16px;'><strong>Estimated Total:</strong> <span style='font-size: 20px; color: #dc3545; font-weight: bold;'>{dto.TotalPrice}€</span></p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        
                        <h3 style='color: #222; margin-bottom: 10px;'>Locations</h3>
                        <p style='margin: 4px 0;'><strong>Pickup Location(s):</strong><br/>{pickupAddresses}</p>
                        <p style='margin: 8px 0 4px 0;'><strong>Dropoff Location:</strong><br/>{dropoffAddress}</p>
                        
                        {(string.IsNullOrWhiteSpace(dto.Notes) ? "" : $@"
                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        <h3 style='color: #222; margin-bottom: 10px;'>Additional Notes</h3>
                        <p style='margin: 4px 0; background: #f8f9fa; padding: 10px; border-radius: 4px;'>{dto.Notes}</p>
                        ")}
                    </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();
            await SendEmailAsync(message);
        }

        public async Task SendAdminNewOfferEmailAsync(CreateOfferDto dto, Guid offerId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress("Gamana Admin", _settings.AdminEmail));
            message.Subject = $"📩 New Offer Request #{offerId.ToString()[..8]} - {dto.FullName}";

            var departureAddress = dto.DepartureAddress != null
                ? $"<strong>{dto.DepartureAddress.Label}</strong> (Floor: {dto.DepartureAddress.Floor}, Elevator: {(dto.DepartureAddress.HasElevator ? "Yes" : "No")})"
                : "N/A";

            var destinationAddress = dto.DestinationAddress != null
                ? $"<strong>{dto.DestinationAddress.Label}</strong> (Floor: {dto.DestinationAddress.Floor}, Elevator: {(dto.DestinationAddress.HasElevator ? "Yes" : "No")})"
                : "N/A";

            var selectedServices = dto.ServiceIds.Count > 0
                ? string.Join(", ", dto.ServiceIds.Select(id => $"Service #{id}"))
                : "None";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px;'>
                        <h2 style='color: #0d6efd; margin-top: 0;'>📩 New Offer Request Received!</h2>
                        <p style='font-size: 14px; color: #666;'><strong>Offer ID:</strong> {offerId}</p>
                        
                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        
                        <h3 style='color: #222; margin-bottom: 10px;'>Customer Details</h3>
                        <p style='margin: 4px 0;'><strong>Name:</strong> {dto.FullName}</p>
                        <p style='margin: 4px 0;'><strong>Email:</strong> <a href='mailto:{dto.Email}' style='color: #0056b3;'>{dto.Email}</a></p>
                        <p style='margin: 4px 0;'><strong>Phone:</strong> <a href='tel:{dto.Phone}' style='color: #0056b3;'>{dto.Phone}</a></p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        
                        <h3 style='color: #222; margin-bottom: 10px;'>Requested Details</h3>
                        <p style='margin: 4px 0;'><strong>Desired Moving Date:</strong> {dto.DesiredMovingDate:g}</p>
                        <p style='margin: 4px 0;'><strong>Requested Services:</strong> {selectedServices}</p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        
                        <h3 style='color: #222; margin-bottom: 10px;'>Locations</h3>
                        <p style='margin: 4px 0;'><strong>Departure Address:</strong><br/>{departureAddress}</p>
                        <p style='margin: 8px 0 4px 0;'><strong>Destination Address:</strong><br/>{destinationAddress}</p>
                        
                        {(string.IsNullOrWhiteSpace(dto.AdditionalInfo) ? "" : $@"
                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>
                        <h3 style='color: #222; margin-bottom: 10px;'>Additional Info</h3>
                        <p style='margin: 4px 0; background: #f8f9fa; padding: 10px; border-radius: 4px;'>{dto.AdditionalInfo}</p>
                        ")}
                    </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();
            await SendEmailAsync(message);
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}