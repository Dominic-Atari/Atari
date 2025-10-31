using GlobalTicket.Core.Common;

namespace GlobalTicket.Core.Entities;

public class Event : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}