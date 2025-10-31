using GlobalTicket.Core.Common;

namespace GlobalTicket.Core.Entities;

public class Order : AuditableEntity
{
    public int UserId { get; set; }
    public int OrderTotal { get; set; }
    public DateTime OrderPlaced { get; set; }
    public bool OrderPaid { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
}