using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WishlistService.Exceptions;
using WishlistService.Models;
using WishlistService.Services;

namespace WishlistService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;
        private readonly ILogger<WishlistController> logger;
        public WishlistController(IWishlistService _wishlistService, ILogger<WishlistController> _logger)
        {
            wishlistService = _wishlistService;
            logger = _logger;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetWishlistByUserName(string userName)
        {
            try
            {
                logger.LogInformation("Getting Wishlist of the User By UserName");
                var result =await  wishlistService.GetWishlistByUserNameAsync(userName);
                return Ok(result);
            }
            catch (WishlistNotFoundException ex)
            {
                logger.LogError($"WishlistNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
        [HttpPost("{userName}")]
        public async Task<IActionResult> AddEventToWishlist(string userName,[FromBody]EventItem eventItem)
        {
            try
            {
                logger.LogInformation("Adding event to the wishlist By UserName");
                var result = await wishlistService.AddEventToWishlistAsync(userName,eventItem);
                return new ObjectResult(result){StatusCode=StatusCodes.Status201Created };
            }
            catch (EventAlreadyExistsException ex)
            {
                logger.LogError($"EventAlreadyExistsException: {ex.Message}");
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{userName}/{eventid}")]
        public async Task<IActionResult> DeleteEventFromWishlist(string userName, int eventid)
        {
            try
            {
                logger.LogInformation("Removing event by userName and event id from wishlist");
                var result = await wishlistService.DeleteEventFromWishlistAsync(userName, eventid);
                return Ok(result);
            }
            catch (EventNotFoundException ex)
            {
                logger.LogError($"EventNotFoundException: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (WishlistNotFoundException ex)
            {
                logger.LogError($"WishlistNotFoundException: {ex.Message}");
                return NotFound(ex.Message); 
            }
        }
         

    }
}
