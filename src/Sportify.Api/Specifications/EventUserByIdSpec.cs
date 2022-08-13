using Ardalis.Specification;
using DomainEntities.EventEntities;

namespace SportifyWebApi.Specifications
{
    public class EventUserByIdSpec : Specification<EventUser>
    {
        public EventUserByIdSpec(int EventId, int UserId)
        {
            Query.Where(x => x.EventId == EventId && x.UserId == UserId);
        }
    }
}
