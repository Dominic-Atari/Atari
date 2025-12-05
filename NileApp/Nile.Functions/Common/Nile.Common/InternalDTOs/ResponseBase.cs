using Nile.Common.Errors;

namespace Nile.Common.InternalDTOs
{
    public abstract class ResponseBase
    {
        public ErrorBase Error { get; set; } = null!;
    }
}