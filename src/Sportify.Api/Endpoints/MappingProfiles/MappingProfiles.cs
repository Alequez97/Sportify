//using AutoMapper;
using AutoMapper;
using Sportify.DomainEntities.EventEntities;

namespace SportifyWebApi.Endpoints.MappingProfiles;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    CreateMap<Event, Event>();
  }
}
