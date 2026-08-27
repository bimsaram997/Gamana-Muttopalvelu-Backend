using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Enums;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public class EmailBackgroundWorker : BackgroundService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailBackgroundWorker> _logger;

        public EmailBackgroundWorker(
            IEmailQueue emailQueue,
            IServiceScopeFactory scopeFactory,
            ILogger<EmailBackgroundWorker> logger)
        {
            _emailQueue = emailQueue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Background Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var item = await _emailQueue.DequeueAsync(stoppingToken);

                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    switch (item.Type)
                    {
                        case EmailType.Booking when item.Payload is CreateBookingDto bookingDto:
                            await emailService.SendAdminNewBookingEmailAsync(bookingDto, item.EntityId);
                            _logger.LogInformation("Sent booking email for ID: {Id}", item.EntityId);
                            break;

                        case EmailType.Offer when item.Payload is CreateOfferDto offerDto:
                            await emailService.SendAdminNewOfferEmailAsync(offerDto, item.EntityId);
                            _logger.LogInformation("Sent offer email for ID: {Id}", item.EntityId);
                            break;

                        default:
                            _logger.LogWarning("Unknown email payload type or mismatch for ID: {Id}", item.EntityId);
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing background email.");
                }
            }

            _logger.LogInformation("Email Background Worker stopped.");
        }
    }
}
