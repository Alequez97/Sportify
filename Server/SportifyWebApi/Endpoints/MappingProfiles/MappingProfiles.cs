//using AutoMapper;
using AutoMapper;
using DomainEntities;

namespace SportifyWebApi.Endpoints.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Event, Event>();
        }
    }
}
