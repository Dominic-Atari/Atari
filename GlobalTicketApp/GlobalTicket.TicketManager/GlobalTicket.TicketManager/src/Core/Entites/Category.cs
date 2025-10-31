using GlobalTicket.Core.Common;

namespace GlobalTicket.Core.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}