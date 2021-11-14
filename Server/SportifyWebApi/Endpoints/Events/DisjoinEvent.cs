using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class DisjoinEvent : BaseAsyncEndpoint
        .WithRequest<DisjoinEventRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;

        public DisjoinEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("/api/events/disjoin")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync(DisjoinEventRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var record = _context.EventUsers.Where(x => x.EventId == request.EventId && x.UserId == userId).FirstOrDefault();

                _context.EventUsers.Remove(record);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    public class DisjoinEventRequest
    {
        public int EventId { get; set; }
    }
}
