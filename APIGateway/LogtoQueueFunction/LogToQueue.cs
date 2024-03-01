using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogtoQueueFunction
{
    public class LogToQueue
    {
        [FunctionName("LogToQueue")]
        public static async Task Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            // Modify the function to handle JSON content and log messages
            try
            {
                //await Task.Delay(30000);
                var info = JsonConvert.DeserializeObject<LoginInfo>(myQueueItem);
                log.LogInformation($"Recieved Successful login message for user: {info.Username}");              
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing queue item: {ex.Message}");
            }
        }
        public class LoginInfo
        {
            public string Username { get; set; }
        }
        
    }
}
