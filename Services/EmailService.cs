using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.DTO.Admin.bookings;
using Gamana_Muttopalvelu_Backend.Options;
using Microsoft.Extensions.Options;

using Resend;


namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IEmailService
    {
        Task SendAdminNewBookingEmailAsync(CreateBookingDto dto, Guid bookingId);
        Task SendAdminNewOfferEmailAsync(CreateOfferDto dto, Guid offerId);

        // Customer emails
        Task SendCustomerBookingReceivedEmailAsync(CreateBookingDto dto, Guid bookingId);
        Task SendCustomerOfferReceivedEmailAsync(CreateOfferDto dto, Guid offerId);
        Task SendCustomerBookingStatusUpdateEmailAsync(UpdateBookingDto dto, Guid bookingId);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IResend _resend;

        public EmailService(
            IResend resend,
             IOptions<EmailSettings> settings)
        {
            _resend = resend;
            _settings = settings.Value;
        }

        public async Task SendAdminNewBookingEmailAsync(
            CreateBookingDto dto,
            Guid bookingId)
        {
            var pickupAddresses = string.Join(
                "<br/>",
                dto.PickupLocations.Select(p =>
                    $"• <strong>{p.Label}</strong> " +
                    $"(Floor: {p.Floor}, Elevator: {(p.HasElevator ? "Yes" : "No")})"
                ));

            var dropoffAddress = dto.DropoffLocation != null
                ? $"<strong>{dto.DropoffLocation.Label}</strong> " +
                  $"(Floor: {dto.DropoffLocation.Floor}, " +
                  $"Elevator: {(dto.DropoffLocation.HasElevator ? "Yes" : "No")})"
                : "N/A";

            var htmlBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px;'>
                    
                    <h2 style='color: #dc3545; margin-top: 0;'>
                        🚨 New Booking Received!
                    </h2>

                    <p style='font-size: 14px; color: #666;'>
                        <strong>Booking ID:</strong> {bookingId}
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                    <h3 style='color: #222; margin-bottom: 10px;'>
                        Customer Details
                    </h3>

                    <p style='margin: 4px 0;'>
                        <strong>Name:</strong> {dto.FullName}
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Email:</strong>
                        <a href='mailto:{dto.Email}' style='color: #0056b3;'>
                            {dto.Email}
                        </a>
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Phone:</strong>
                        <a href='tel:{dto.Phone}' style='color: #0056b3;'>
                            {dto.Phone}
                        </a>
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                    <h3 style='color: #222; margin-bottom: 10px;'>
                        Service Details
                    </h3>

                    <p style='margin: 4px 0;'>
                        <strong>Service Date:</strong> {dto.ServiceDate:g}
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Package ID:</strong> {dto.SelectedPackageId}
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Estimated Hours:</strong> {dto.EstimatedHours} hrs
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Move-Out Cleaning:</strong>
                        {(dto.IncludeCleaning ? "Yes (+110€)" : "No")}
                    </p>

                    <p style='margin: 8px 0; font-size: 16px;'>
                        <strong>Estimated Total:</strong>
                        <span style='font-size: 20px; color: #dc3545; font-weight: bold;'>
                            {dto.TotalPrice}€
                        </span>
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                    <h3 style='color: #222; margin-bottom: 10px;'>
                        Locations
                    </h3>

                    <p style='margin: 4px 0;'>
                        <strong>Pickup Location(s):</strong><br/>
                        {pickupAddresses}
                    </p>

                    <p style='margin: 8px 0 4px 0;'>
                        <strong>Dropoff Location:</strong><br/>
                        {dropoffAddress}
                    </p>

                    {(string.IsNullOrWhiteSpace(dto.Notes) ? "" : $@"
                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                        <h3 style='color: #222; margin-bottom: 10px;'>
                            Additional Notes
                        </h3>

                        <p style='margin: 4px 0; background: #f8f9fa; padding: 10px; border-radius: 4px;'>
                            {dto.Notes}
                        </p>
                    ")}
                </div>";

            var subject =
                $"New Moving Booking #{bookingId.ToString()[..8]} - {dto.FullName}";

            await SendEmailAsync(_settings.AdminEmail, subject, htmlBody);
        }

        public async Task SendAdminNewOfferEmailAsync(
            CreateOfferDto dto,
            Guid offerId)
        {
            var departureAddress = dto.DepartureAddress != null
                ? $"<strong>{dto.DepartureAddress.Label}</strong> " +
                  $"(Floor: {dto.DepartureAddress.Floor}, " +
                  $"Elevator: {(dto.DepartureAddress.HasElevator ? "Yes" : "No")})"
                : "N/A";

            var destinationAddress = dto.DestinationAddress != null
                ? $"<strong>{dto.DestinationAddress.Label}</strong> " +
                  $"(Floor: {dto.DestinationAddress.Floor}, " +
                  $"Elevator: {(dto.DestinationAddress.HasElevator ? "Yes" : "No")})"
                : "N/A";

            var selectedServices = dto.ServiceIds.Count > 0
                ? string.Join(
                    ", ",
                    dto.ServiceIds.Select(id => $"Service #{id}"))
                : "None";

            var htmlBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px;'>

                    <h2 style='color: #0d6efd; margin-top: 0;'>
                        📩 New Offer Request Received!
                    </h2>

                    <p style='font-size: 14px; color: #666;'>
                        <strong>Offer ID:</strong> {offerId}
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                    <h3 style='color: #222; margin-bottom: 10px;'>
                        Customer Details
                    </h3>

                    <p style='margin: 4px 0;'>
                        <strong>Name:</strong> {dto.FullName}
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Email:</strong>
                        <a href='mailto:{dto.Email}' style='color: #0056b3;'>
                            {dto.Email}
                        </a>
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Phone:</strong>
                        <a href='tel:{dto.Phone}' style='color: #0056b3;'>
                            {dto.Phone}
                        </a>
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                    <h3 style='color: #222; margin-bottom: 10px;'>
                        Requested Details
                    </h3>

                    <p style='margin: 4px 0;'>
                        <strong>Desired Moving Date:</strong>
                        {dto.DesiredMovingDate:g}
                    </p>

                    <p style='margin: 4px 0;'>
                        <strong>Requested Services:</strong>
                        {selectedServices}
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                    <h3 style='color: #222; margin-bottom: 10px;'>
                        Locations
                    </h3>

                    <p style='margin: 4px 0;'>
                        <strong>Departure Address:</strong><br/>
                        {departureAddress}
                    </p>

                    <p style='margin: 8px 0 4px 0;'>
                        <strong>Destination Address:</strong><br/>
                        {destinationAddress}
                    </p>

                    {(string.IsNullOrWhiteSpace(dto.AdditionalInfo) ? "" : $@"
                        <hr style='border: none; border-top: 1px solid #eee; margin: 15px 0;'/>

                        <h3 style='color: #222; margin-bottom: 10px;'>
                            Additional Info
                        </h3>

                        <p style='margin: 4px 0; background: #f8f9fa; padding: 10px; border-radius: 4px;'>
                            {dto.AdditionalInfo}
                        </p>
                    ")}
                </div>";

            var subject =
                $"📩 New Offer Request #{offerId.ToString()[..8]} - {dto.FullName}";

            await SendEmailAsync(_settings.AdminEmail,subject, htmlBody);
        }

        public async Task SendCustomerBookingReceivedEmailAsync(
    CreateBookingDto dto,
    Guid bookingId)
        {
            var subject = "Gamana Muuttopalvelu - Booking Received";

            var htmlBody = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; max-width: 600px; margin: 0 auto;'>

            <h2 style='color: #dc3545;'>
                Thank you for your booking request!
            </h2>

            <p>
                Hello <strong>{dto.FullName}</strong>,
            </p>

            <p>
                We have received your booking request.
            </p>

            <p>
                Our team will review your request and contact you soon.
            </p>

            <p>
                <strong>Booking ID:</strong> {bookingId}
            </p>

            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />

            <p>
                If you have any questions, please contact us.
            </p>

            <p>
                Best regards,<br />
                <strong>Gamana Muuttopalvelu</strong>
            </p>

        </div>";

            await SendEmailAsync(
                dto.Email,
                subject,
                htmlBody);
        }

        public async Task SendCustomerOfferReceivedEmailAsync(
    CreateOfferDto dto,
    Guid offerId)
        {
            var subject = "Gamana Muuttopalvelu - Offer Request Received";

            var htmlBody = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; max-width: 600px; margin: 0 auto;'>

            <h2 style='color: #0d6efd;'>
                Thank you for your offer request!
            </h2>

            <p>
                Hello <strong>{dto.FullName}</strong>,
            </p>

            <p>
                We have received your offer request.
            </p>

            <p>
                Our team will review your request and contact you soon.
            </p>

            <p>
                <strong>Request ID:</strong> {offerId}
            </p>

            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />

            <p>
                If you have any questions, please contact us.
            </p>

            <p>
                Best regards,<br />
                <strong>Gamana Muuttopalvelu</strong>
            </p>

        </div>";

            await SendEmailAsync(
                dto.Email,
                subject,
                htmlBody);
        }

        public async Task SendCustomerBookingStatusUpdateEmailAsync(UpdateBookingDto dto, Guid bookingId  )
        {
            // Configure badge colors and titles based on status
            var (statusColor, badgeBg, statusTitle) = dto.Status.ToLower() switch
            {
                "confirmed" => ("#198754", "#d1e7dd", "✅ Booking Confirmed"),
                "completed" => ("#0d6efd", "#cfe2ff", "🎉 Booking Completed"),
                "cancelled" or "canceled" => ("#dc3545", "#f8d7da", "❌ Booking Cancelled"),
                _ => ("#ffc107", "#fff3cd", "⏳ Booking Update") // Default for Pending or other statuses
            };

            var statusMessage = dto.Status.ToLower() switch
            {
                "confirmed" => "Great news! Your booking has been confirmed by our team. We are looking forward to assisting you on your moving day.",
                "completed" => "Your moving service has been marked as completed. Thank you for choosing Gamana Muuttopalvelu!",
                "cancelled" or "canceled" => "Your booking has been cancelled. If you believe this was done in error or if you wish to reschedule, please contact us.",
                _ => "There has been an update regarding your booking status."
            };

            var adminNotesHtml = string.IsNullOrWhiteSpace(dto.AdminNotes) ? "" : $@"
        <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'/>

        <h3 style='color: #222; margin-bottom: 8px;'>
            Message from Admin
        </h3>

        <div style='background: #f8f9fa; border-left: 4px solid {statusColor}; padding: 12px 16px; border-radius: 4px; font-style: italic; color: #444;'>
            {dto.AdminNotes}
        </div>";

            var htmlBody = $@"
        <div style='font-family: Arial, sans-serif; padding: 24px; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px;'>
            <div style='text-align: center; margin-bottom: 20px;'>
                <span style='background-color: {badgeBg}; color: {statusColor}; font-size: 14px; font-weight: bold; padding: 6px 16px; border-radius: 20px; display: inline-block;'>
                    {dto.Status.ToUpper()}
                </span>
            </div>

            <h2 style='color: #222; margin-top: 0; text-align: center;'>
                {statusTitle}
            </h2>

            <p style='font-size: 15px; color: #444; line-height: 1.5;'>
                Hi <strong>{dto.FullName}</strong>,
            </p>

            <p style='font-size: 15px; color: #444; line-height: 1.5;'>
                {statusMessage}
            </p>

            <div style='background-color: #f1f3f5; padding: 12px 16px; border-radius: 6px; margin: 20px 0;'>
                <p style='margin: 0; font-size: 14px; color: #555;'>
                    <strong>Booking ID:</strong> {bookingId}
                </p>
                <p style='margin: 4px 0 0 0; font-size: 14px; color: #555;'>
                    <strong>Current Status:</strong> <span style='color: {statusColor}; font-weight: bold;'>{dto.Status}</span>
                </p>
            </div>

            {adminNotesHtml}

            <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0 16px 0;'/>

            <p style='font-size: 13px; color: #777; text-align: center; margin: 0;'>
                If you have any questions, feel free to reply directly to this email.
            </p>
            <p style='font-size: 13px; color: #777; text-align: center; margin-top: 4px;'>
                <strong>Gamana Muuttopalvelu</strong>
            </p>
        </div>";

            var subject = $"Booking #{bookingId.ToString()[..8]} Update - {dto.Status}";

            await SendEmailAsync(dto.Email, subject, htmlBody);
        }

        private async Task SendEmailAsync(
      string recipient,
      string subject,
      string htmlBody)
        {
            var message = new EmailMessage
            {
                From = "Gamana Muuttopalvelu <noreply@gamanamuutto.fi>",
                // Sets where customer replies are delivered
                ReplyTo = "gamanamuutto@gmail.com", // Replace with your actual active support email
                Subject = subject,
                HtmlBody = htmlBody
            };

            message.To.Add(recipient);

            try
            {
                var response = await _resend.EmailSendAsync(message);

                Console.WriteLine($"RESEND SUCCESS: {response.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RESEND ERROR: {ex}");
                throw;
            }
        }
    }
}