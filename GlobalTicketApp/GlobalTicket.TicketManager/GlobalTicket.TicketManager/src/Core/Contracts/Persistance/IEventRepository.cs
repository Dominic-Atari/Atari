using GlobalTicket.Core.Entities;

namespace GlobalTicket.Core.Contracts.Persistence
{
    public interface IEventRepository : IAsyncRepository<Event>
    {
    }
}