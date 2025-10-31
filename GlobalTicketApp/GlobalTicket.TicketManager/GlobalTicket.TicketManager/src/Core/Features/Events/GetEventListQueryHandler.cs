using AutoMapper;
using GlobalTicket.Core.Contracts.Persistence;
using GlobalTicket.Core.Contracts.Persistence.Features.Events;
using GlobalTicket.Core.Entities;
using MediatR;

namespace GlobalTicket.Core.Features.Events
{
    public class GetEventListQueryHandler : IRequestHandler<GetEventListQuery, List<EventListVm>>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public GetEventListQueryHandler(IAsyncRepository<Event> eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<List<EventListVm>> Handle(GetEventListQuery request, CancellationToken cancellationToken)
        {
            var eventList = await _eventRepository.ListAllAsync();
            return _mapper.Map<List<EventListVm>>(eventList);
        }
    }
}
