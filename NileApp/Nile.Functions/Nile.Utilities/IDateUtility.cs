namespace Nile.Utilities;

public interface IDateUtility
{
    DateOnly CetralTimeNow { get; }
    DateTime UtcNow { get; }
    DateTimeOffset UtcNowOffset { get; }
    
}