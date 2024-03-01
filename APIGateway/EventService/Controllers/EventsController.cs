using System.Net.Http;
using EventService.Exceptions;
using EventService.Models;
using Newtonsoft.Json;
using EventService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService eventService;
        private readonly ILogger<EventsController> logger;
        public EventsController(IEventService service, ILogger<EventsController> _logger)
        {
            eventService = service;
            logger = _logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                logger.LogInformation("Fetching All Events from SeatGeek Event External Api");
                var result = await eventService.GetEventsAsync();
                return Ok(result);
            }
            catch (FetchingEventsException ex)
            {
                logger.LogError($"FetchingEventsException: {ex.Message}");
                return Conflict(ex.Message);
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                logger.LogInformation("Fetching Event Details BY event-id from SeatGeek Event External Api");
                var result = await eventService.GetEventItemById(id);
                return Ok(result);
            }
            catch (EventNotFoundException ex)
            {
                logger.LogError($"EventNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (FetchingEventsException ex)
            {
                logger.LogError($"FetchingEventsException: {ex.Message}");
                return Conflict(ex.Message); 
            }

        }
    }
}

