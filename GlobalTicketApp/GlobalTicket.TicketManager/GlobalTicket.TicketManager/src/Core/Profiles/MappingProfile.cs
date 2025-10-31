using AutoMapper;
using GlobalTicket.Core.Contracts.Persistence.Features.Events;
using GlobalTicket.Core.Entities;

namespace GlobalTicket.Core.Contracts.Pesistance
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventListVm>().ReverseMap();
            CreateMap<Event, EventDetailVm>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}