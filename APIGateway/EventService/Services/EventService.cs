using EventService.Exceptions;
using EventService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EventService.Services
{
    public class EventService:IEventService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration configuration;
        public EventService(IHttpClientFactory httpClientFactory, IConfiguration _configuration) 
        { 
            _httpClientFactory=httpClientFactory;
            configuration = _configuration;
        }

        public async Task<EventDataResponse> GetEventsAsync()
        {
            try
            {
                string? apiUrl = configuration.GetSection("ExternalAPI:apiUrl").Value;
                string? clientId = configuration.GetSection("ExternalAPI:clientId").Value;
                string? clientSecret = configuration.GetSection("ExternalAPI:clientSecret").Value;

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}?client_id={clientId}&client_secret={clientSecret}");
                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        // Process responseData or return it directly
                        var data = JsonConvert.DeserializeObject<EventDataResponse>(responseData);
                        return data;
                    }
                    else
                    {
                        throw new FetchingEventsException($"Something went wrong not able to fetch events");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new FetchingEventsException($"Internal Server Error 500 {ex.Message}");
            }
        }
        public async Task<EventItem> GetEventItemById(int id)
        {
            try
            {
                string? apiUrl = configuration.GetSection("ExternalAPI:apiUrl").Value;
                string? clientId = configuration.GetSection("ExternalAPI:clientId").Value;
                string? clientSecret = configuration.GetSection("ExternalAPI:clientSecret").Value;

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/{id}?client_id={clientId}&client_secret={clientSecret}");
                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<EventItem>(responseData);

                        // Process responseData or return it directly
                        return data;
                    }
                    else
                    {
                        throw new EventNotFoundException($"Event with id: {id} doesn't exist");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new FetchingEventsException($"Internal Server Error-500 {ex.Message}");
            }
        }

       
    }
}
