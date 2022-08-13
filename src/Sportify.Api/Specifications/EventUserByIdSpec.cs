using Ardalis.Specification;
using Sportify.DomainEntities.EventEntities;

namespace Sportify.Api.Specifications;

public class EventUserByIdSpec : Specification<EventUser>
{
  public EventUserByIdSpec(int EventId, int UserId)
  {
    Query.Where(x => x.EventId == EventId && x.UserId == UserId);
  }
}
