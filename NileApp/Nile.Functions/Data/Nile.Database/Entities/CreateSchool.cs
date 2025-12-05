namespace Nile.Database.Entities;

public class School
{
    public Guid SchoolId { get; set; }
    public string SchoolName { get; set; } = null!;
    public string SchoolAddress { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string County { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User User { get; set; } = null!;
    
}