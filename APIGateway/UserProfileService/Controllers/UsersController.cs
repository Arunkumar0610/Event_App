using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Exceptions;
using UserProfileService.Models;
using UserProfileService.Services;
namespace UserProfileService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService ;
        private readonly ILogger<UsersController> logger;
        public UsersController(IUserService _userService, ILogger<UsersController> _logger) 
        {
           userService = _userService;
           logger = _logger;
        }
        [HttpGet("{id}",Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                logger.LogInformation("Getting user By id");
                var user = await userService.GetUserById(id);
                return Ok(user);
            }
            catch(UserNotFoundException ex)
            {
                logger.LogError($"UserNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
        [HttpGet("byname/{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            try
            {
                logger.LogInformation("Getting user By userName");
                var user = await userService.GetUserByUserName(userName);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"UserNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]Users user)
        {
            try
            {
                logger.LogInformation("Registering user and sending userDetails to kafka messagebus ");
                var registeredUser = await userService.RegisterUser(user);
                return CreatedAtAction(nameof(GetUserById), new{ id= registeredUser.UserId}, registeredUser);
            }
            catch (UserAlreadyExistException ex)
            {
                logger.LogError($"UserAlreadyExistException: {ex.Message}");
                return Conflict(ex.Message);
            }
            catch (KafkaProducerException ex)
            {
                logger.LogError($"KafkaProducerException: {ex.Message}");
                return Conflict(ex.Message);
            }
        }
    }
}
