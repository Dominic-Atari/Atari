using GlobalTicket.Core.Entities;

namespace GlobalTicket.Core.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
    }
}