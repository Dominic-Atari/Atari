namespace Nile.Database.Entities;

public class Account
{
    public Guid AccountId { get; init; }

    public Guid UserId { get; init; }

    public string EmailAddress { get; init; } = null!;

    public string ExternalAuthId { get; init; } = null!;

    public Guid ExternalCustomerId { get; init; }
    
    public DateTimeOffset CreatedAt { get; init; }
    
    public DateTimeOffset UpdatedAt { get; set; }
    
    public User User { get; set; } = null!;
}