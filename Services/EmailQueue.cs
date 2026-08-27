using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Enums;
using Gamana_Muttopalvelu_Backend.Models;
using System.Threading.Channels;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IEmailQueue
    {
        void QueueEmail<T>(EmailType type, T dto, Guid entityId) where T : class;
        ValueTask<EmailWorkItem> DequeueAsync(CancellationToken cancellationToken);
    }

    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<EmailWorkItem> _queue = Channel.CreateUnbounded<EmailWorkItem>();

        public void QueueEmail<T>(EmailType type, T dto, Guid entityId) where T : class
        {
            _queue.Writer.TryWrite(new EmailWorkItem
            {
                Type = type,
                EntityId = entityId,
                Payload = dto
            });
        }

        public async ValueTask<EmailWorkItem> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
