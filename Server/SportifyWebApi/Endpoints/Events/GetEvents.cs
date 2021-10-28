using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportifyWebApi.Endpoints.Events
{
    public class GetEvents : BaseAsyncEndpoint
        .WithRequest<GetEventsRequest>
        .WithResponse<List<Event>>
    {
        private readonly SportifyDbContext _context;

        public GetEvents(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/events")]
        public override async Task<ActionResult<List<Event>>> HandleAsync([FromRoute] GetEventsRequest request, CancellationToken cancellationToken = default)
        {
            //var q = _context.Events.Include(v => v.Venue);
            //if (!string.IsNullOrEmpty(request.SortBy))
            //{
            //    //q = q.OrderBy(x=)
            //}
            return await _context.Events.Include(v => v.Venue).ToListAsync();
        }
    }

    public class GetEventsRequest
    {
        [FromQuery(Name ="sortBy")] public string SortBy { get; set; }
        [FromQuery(Name = "sortDesc")] public bool SortDesc { get; set; }
    }
}
