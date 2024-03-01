using WishlistService.Models;

namespace WishlistService.Services
{
    public interface IWishlistService
    {
        public Task<Wishlist> GetWishlistByUserNameAsync(string UserName);
        public Task<EventItem> AddEventToWishlistAsync(string UserName, EventItem eventItem);
        public Task<bool> DeleteEventFromWishlistAsync(string UserName, int id);
    }
}
