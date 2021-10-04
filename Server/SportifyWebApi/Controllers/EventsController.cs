using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportifyWebApi.Controllers
{
    public class EventsController : BaseApiController
    {
        private readonly IDataRepository<Event> _eventsDataRepository;

        public EventsController(IDataRepository<Event> dataRepository)
        {
            _eventsDataRepository = dataRepository;
        }

        [HttpGet]
        public ActionResult<Event> Get(int id)
        {
            var @event = _eventsDataRepository.Read(id);
            return @event != null ? Ok(@event) : NotFound();
        }

        [HttpPost]
        public ActionResult<Event> Post([FromBody] Event @event)
        {
            if (@event == null)
            {
                return new JsonResult("Provide correct information about event") { StatusCode = StatusCodes.Status400BadRequest };
            }

            var resultIsSuccess = _eventsDataRepository.Create(@event);
            if (resultIsSuccess)
            {
                return new JsonResult("Event successefully saved") { StatusCode = StatusCodes.Status200OK };
            }

            return new JsonResult($"Failed to save event")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        [Route("api/events/find-by-title")]
        [HttpGet]
        public ActionResult<Event> Get([FromQuery] string title)
        {
            var events = _eventsDataRepository.FindByExpression(e => e.Title == title, e => e.Venue);
            if (events != null && events.Count > 0)
            {
                return new JsonResult(events) { StatusCode = StatusCodes.Status200OK };
            }

            return new JsonResult($"Fail to find user with title {title}")
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }
    }
}
