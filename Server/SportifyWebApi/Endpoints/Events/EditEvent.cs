using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class EditEvent : BaseAsyncEndpoint
        .WithRequest<EditEventRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;
        private IMapper _mapper;

        public EditEvent(SportifyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPut("/api/event/edit/{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody] EditEventRequest request, CancellationToken cancellationToken = default)
        {
            var @event = await _context.Events.FindAsync(request.Id);

            _mapper.Map(request, @event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return BadRequest();
            }
            
            return Ok();
        }
    }

    public class EditEventRequest
    {
        [FromRoute] 
        public int Id { get; set; }

        [FromBody]
        public string Title { get; set; }

        [FromBody]
        public int CategoryId { get; set; }

        [FromBody]
        public string BriefDesc { get; set; }

        [FromBody]
        public string Description { get; set; }

        [FromBody]
        public int CountryId { get; set; }

        [FromBody]
        public int CityId { get; set; }

        [FromBody]
        public string Address { get; set; }

        [FromBody]
        public DateTime TimeOfTheEvent { get; set; }
    }
}
