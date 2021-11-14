using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities.EventEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody]JoinEventRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.EventUsers.Add(new EventUser
                {
                    EventId = request.EventId,
                    UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                });

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
