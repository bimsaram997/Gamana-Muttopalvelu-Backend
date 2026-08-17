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
                    // Dequeue next job asynchronously (waits until an item is available)
                    var (dto, bookingId) = await _emailQueue.DequeueAsync(stoppingToken);

                    // Scoped services (like DbContext or EmailService) must be resolved within a scope
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    await emailService.SendAdminNewBookingEmailAsync(dto, bookingId);
                    _logger.LogInformation("Successfully sent background booking email for Booking ID: {BookingId}", bookingId);
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown requested
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing background email sending.");
                }
            }

            _logger.LogInformation("Email Background Worker stopped.");
        }
    }
}
