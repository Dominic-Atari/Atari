using Microsoft.Extensions.Logging;
using Nile.Common;

namespace Nile.Utilities;

public abstract class UtilityBase : ServiceContractBase
{
    protected UtilityBase(ILogger logger) : base(logger)
    {
    }
}