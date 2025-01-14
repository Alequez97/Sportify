using Ardalis.Specification;
using Sportify.DomainEntities.EventEntities;

namespace Sportify.Api.Specifications;

public class FilterEventsSpec : Specification<Event>
{
  public FilterEventsSpec(int? CategoryId, int? CountryId, int? CityId)
  {
    if (CategoryId != 0)
    {
      Query.Where(x => x.CategoryId == CategoryId);
    }

    if (CountryId != 0)
    {
      Query.Where(x => x.Venue.CountryId == CountryId);
    }

    if (CityId != 0)
    {
      Query.Where(x => x.Venue.CityId == CityId);
    }
  }
}
