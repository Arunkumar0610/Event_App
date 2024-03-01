using WishlistService.Models;

namespace WishlistService.Repositories
{
    public interface IWishlistRepository
    {
        public Task<Wishlist> GetWishlistByUserNameAsync(string UserName);
        public Task<EventItem> AddEventToWishlistAsync(string UserName, EventItem eventItem);
        public Task<bool> DeleteEventFromWishlistAsync(string UserName, int id);


    }
}
