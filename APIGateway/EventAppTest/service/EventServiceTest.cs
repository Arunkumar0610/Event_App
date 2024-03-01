using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using EventService.Exceptions;
using EventService.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using WishlistService.Repositories;
namespace EventAppTest.service
{
    public class EventServiceTest
    {
        private Mock<IHttpClientFactory> httpClientFactoryMock;
        private EventService.Services.EventService eventService;
        private Mock<IConfiguration> configurationMock;
        [SetUp]
        public void Setup()
        {
            httpClientFactoryMock = new Mock<IHttpClientFactory>();
            configurationMock = new Mock<IConfiguration>();
            eventService = new EventService.Services.EventService(httpClientFactoryMock.Object, configurationMock.Object);
        }
        //[Test]
        //public async Task GetEventsAsync_ShouldReturnEventDataResponse_WhenApiCallIsSuccessful()
        //{
        //    // Arrange
        //    var eventDataResponse = new EventDataResponse { /* your sample data */ };
        //    var jsonString = JsonConvert.SerializeObject(eventDataResponse);
        //    configurationMock.Setup(c => c.GetSection("ExternalAPI:apiUrl").Value).Returns("apiUrl");
        //    configurationMock.Setup(c => c.GetSection("ExternalAPI:clientId").Value).Returns("clientId");
        //    configurationMock.Setup(c => c.GetSection("ExternalAPI:clientSecret").Value).Returns("clientSecret");
        //    // Set up other configuration values as needed
        //    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
        //    };
        //    var httpClientMock = new Mock<HttpClient>();
        //    httpClientMock.Setup(hc => hc.SendAsync(It.IsAny<HttpRequestMessage>()))
        //           .ReturnsAsync(httpResponseMessage);
        //    httpClientFactoryMock.Setup(f => f.CreateClient()).Returns(httpClientMock.Object);            
        //    // Act
        //    var result = await eventService.GetEventsAsync();
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.IsInstanceOf<EventDataResponse>(result);
        //    // Add more assertions as needed
        //}
        //[Test]
        //public void GetEventsAsync_ShouldThrowFetchingEventsException_WhenApiCallFails()
        //{
        //    // Arrange
        //    configurationMock.Setup(c => c.GetSection("ExternalAPI:apiUrl").Value).Returns("https://api.seatgeek.com/2/events");
        //    // Set up other configuration values as needed
        //    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    var httpClientMock = new Mock<HttpClient>();
        //    httpClientMock.Setup(hc => hc.SendAsync(It.IsAny<HttpRequestMessage>()))
        //           .ReturnsAsync(httpResponseMessage);
        //    httpClientFactoryMock.Setup(f => f.CreateClient()).Returns(httpClientMock.Object);
            
        //    // Act & Assert
        //    Assert.ThrowsAsync<FetchingEventsException>(async () => await eventService.GetEventsAsync());
        //}
    }
}
