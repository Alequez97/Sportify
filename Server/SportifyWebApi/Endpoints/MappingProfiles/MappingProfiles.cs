using AutoMapper;
using DomainEntities;

namespace BusinessLogic
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Event, Event>();
        }
    }
}
