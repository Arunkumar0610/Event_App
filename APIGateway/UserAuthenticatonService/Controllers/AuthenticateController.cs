using System.Diagnostics.Eventing.Reader;
using System.Security.Authentication;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using UserAuthenticatonService.Exceptions;
using UserAuthenticatonService.Models;
using UserAuthenticatonService.Services;
namespace UserAuthenticatonService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthenticateController> logger;
        public AuthenticateController(IAuthService _authService, ILogger<AuthenticateController> _logger)
        {
           authService = _authService;
           logger=_logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(Login login)
        {
            try
            {
                logger.LogInformation("Login User");
                var loginresponse = await authService.Login(login);

                //// Log successful login to the Azure Function
                await LogToQueue(login.UserName);
                return Ok(loginresponse);
            }
            catch (InvalidUserCredentialsException ex)
            {
                logger.LogError($"InvalidUserCredentialsException: {ex.Message}");
                return Unauthorized(ex.Message);
            }
        }
        private async Task LogToQueue(string username)
        {
            var storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=blobstorageaccountarun;AccountKey=FYpmd2nrSdtnH5LQa15KQwEqEM4NfOibhrcyJjz0GMaVUY1GUsTCLTuFLSi1Bxm69sif/UUt20jb+AStzDi+4g==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAcccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudQueueClient queueClient = storageAcccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("myqueue-items");
            var loginInfo=new LoginInfo { Username= username };
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(loginInfo));
            var delayTime = TimeSpan.FromSeconds(50);
            await queue.AddMessageAsync(message,null,delayTime,null,null);
        }
        public class LoginInfo
        {
            public string Username { get; set; }
        }
    }
}
