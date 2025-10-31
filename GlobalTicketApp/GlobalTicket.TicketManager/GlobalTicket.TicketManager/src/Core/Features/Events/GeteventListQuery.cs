using   MediatR;

namespace GlobalTicket.Core.Contracts.Persistence.Features.Events

{
    public class GetEventListQuery : IRequest<List<EventListVm>>
    {
    }
}