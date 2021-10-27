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
        .WithoutRequest
        .WithResponse<List<Event>>
    {
        private readonly SportifyDbContext _context;

        public GetEvents(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/events")]
        public override async Task<ActionResult<List<Event>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Events.Include(v => v.Venue).ToListAsync();
        }
    }
}
