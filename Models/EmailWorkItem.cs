using Gamana_Muttopalvelu_Backend.Enums;

namespace Gamana_Muttopalvelu_Backend.Models
{
    public class EmailWorkItem
    {
        public EmailType Type { get; set; }
        public Guid EntityId { get; set; }
        public object Payload { get; set; } = null!;
    }
}
