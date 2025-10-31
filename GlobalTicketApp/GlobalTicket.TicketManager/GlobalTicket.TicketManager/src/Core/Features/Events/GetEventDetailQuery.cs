using MediatR;
using GlobalTicket.Core.Contracts.Persistence.Features.Events;

namespace GlobalTicket.TicketManager.Core.Contracts.Persistence.Features.Events
{
    public class GetEventDetailQuery : IRequest<EventDetailVm>
    {
        public Guid Id  { get; init; }
    }
}