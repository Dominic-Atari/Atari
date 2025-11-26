using Microsoft.Extensions.Logging;
using Nile.Utilities;

namespace Nile.Accessors;

internal class DateUtility : UtilityBase, IDateUtility
{
    public DateUtility(ILogger<DateUtility> logger) : base(logger)
    {
    }
    public DateOnly CetralTimeNow => ToCentralDateOnly(DateTime.UtcNow);

    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
    
    private DateOnly ToCentralDateOnly(object utcNow)
    {
        throw new NotImplementedException();
    }
}
