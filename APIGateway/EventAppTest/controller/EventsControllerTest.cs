using EventService.Controllers;
using EventService.Exceptions;
using EventService.Models;
using EventService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserAuthenticatonService.Models;

namespace EventAppTest.controller
{
    public class EventsControllerTest
    {
        private Mock<IEventService> eventServiceMock;
        private Mock<ILogger<EventsController>> loggerMock;
        private EventsController eventsController;
        [SetUp]
        public void Setup()
        {
            eventServiceMock = new Mock<IEventService>();
            loggerMock = new Mock<ILogger<EventsController>>();
            eventsController = new EventsController(eventServiceMock.Object, loggerMock.Object);
        }
        #region PositiveTests
        [Test]
        public async Task GetEvents_Returns_OK_Events()
        {
            EventDataResponse events = new()
            {
                Events =new  List<EventItem>(){}
            };
            eventServiceMock.Setup(service => service.GetEventsAsync()).ReturnsAsync(events);
            var actual = await eventsController.GetEvents();
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(events));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
        [Test]
        public async Task GetEventById_Returns_Ok_Event()
        {
            EventItem eventItem = new()
            {
                Id=123456,
                Title="ncc womens football"
            };
            eventServiceMock.Setup(service => service.GetEventItemById(eventItem.Id))
                .ReturnsAsync(eventItem);
            var actual =await eventsController.GetEventById(eventItem.Id);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(eventItem));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

        }


        #endregion

        #region NegativeTests
        [Test]
        public async Task GetEvents_Returns_Conflict_FetchingEvents()
        {
            EventDataResponse events = new()
            {
                Events = new List<EventItem>() { }
            };
            eventServiceMock.Setup(service => service.GetEventsAsync()).Throws<FetchingEventsException>();
            var actual = await eventsController.GetEvents();
            var conflictResult = actual as ObjectResult;
            Assert.That(conflictResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        }
        [Test]
        public async Task GetEventById_Returns_NotFound_Event()
        {
            EventItem eventItem = new()
            {
                Id = 123456,
                Title = "ncc womens football"
            };
            eventServiceMock.Setup(service => service.GetEventItemById(eventItem.Id))
                .Throws<EventNotFoundException>();
            var actual = await eventsController.GetEventById(eventItem.Id);
            var notFoundResult = actual as ObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));

        }
        [Test]
        public async Task GetEventById_Returns_Conflict_FetchingEvent()
        {
            EventItem eventItem = new()
            {
                Id = 123456,
                Title = "ncc womens football"
            };
            eventServiceMock.Setup(service => service.GetEventItemById(eventItem.Id))
                .Throws<FetchingEventsException>();
            var actual = await eventsController.GetEventById(eventItem.Id);
            var conflictResult = actual as ObjectResult;
            Assert.That(conflictResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));

        }
        #endregion
    }
}
