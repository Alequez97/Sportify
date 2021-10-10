using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using DomainEntities;
using BusinessLogic.Events;
using Microsoft.AspNetCore.Authorization;

namespace SportifyWebApi.Controllers
{
    //[Authorize]
    public class EventsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event @event)
        {
            return Ok(await Mediator.Send(new Create.Command { Event = @event }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditEvent(int id, Event @event)
        {
            @event.Id = id;

            return Ok(await Mediator.Send(new Edit.Command { Event = @event }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}
