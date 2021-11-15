using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.Specification.EntityFrameworkCore;
using DataServices;
using DomainEntities.EventEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Specifications;

namespace SportifyWebApi.Endpoints.Events
{
    public class JoinEvent : BaseAsyncEndpoint
        .WithRequest<JoinEventRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;

        public JoinEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("/api/events/join")]
        public override async Task<ActionResult> HandleAsync([FromBody]JoinEventRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var record = _context.EventUsers.WithSpecification(new EventUserByIdSpec(request.EventId, userId)).FirstOrDefault();

                if(record != null)
                {
                    record.IsGoing = true;
                }
                else
                {
                    _context.EventUsers.Add(new EventUser
                    {
                        EventId = request.EventId,
                        UserId = userId,
                        IsGoing = true
                    });
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    public class JoinEventRequest
    {
        public int EventId { get; set; }
    }
}
