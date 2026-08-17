using Gamana_Muttopalvelu_Backend.DTO;
using System.Threading.Channels;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IEmailQueue
    {
        void QueueEmail(CreateBookingDto dto, Guid bookingId);
        Task<(CreateBookingDto Dto, Guid BookingId)> DequeueAsync(CancellationToken cancellationToken);
    }
    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<(CreateBookingDto Dto, Guid BookingId)> _queue =
            Channel.CreateUnbounded<(CreateBookingDto, Guid)>();

        public void QueueEmail(CreateBookingDto dto, Guid bookingId)
        {
            _queue.Writer.TryWrite((dto, bookingId));
        }

        public async Task<(CreateBookingDto Dto, Guid BookingId)> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
