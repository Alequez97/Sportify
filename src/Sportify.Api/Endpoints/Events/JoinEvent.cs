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
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;
using SportifyWebApi.Specifications;
using System.ComponentModel.DataAnnotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class JoinEvent : EndpointBaseAsync
        .WithRequest<JoinEventRequest>
        .WithActionResult
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
                int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
                var eventExists = _context.Events.Any(e => e.Id == request.EventId);

                if (!eventExists) return NotFound();

                var record = _context.EventUsers.WithSpecification(new EventUserByIdSpec((int)request.EventId, userId)).FirstOrDefault();

                if(record != null)
                {
                    record.IsGoing = true;
                }
                else
                {
                    _context.EventUsers.Add(new EventUser
                    {
                        EventId = (int)request.EventId,
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
        [Required]
        public int? EventId { get; set; }
    }
}
