using WishlistService.Exceptions;
using WishlistService.Models;
using WishlistService.Repositories;

namespace WishlistService.Services
{
    public class WishlistService:IWishlistService
    {
        private readonly IWishlistRepository _repository;
        public WishlistService(IWishlistRepository repository) 
        {
            _repository = repository;
        }

        public async Task<EventItem> AddEventToWishlistAsync(string UserName, EventItem eventItem)
        {
           var result= await _repository.AddEventToWishlistAsync(UserName, eventItem);
           if (result != null)
           {
               return result;
           }
           else
           {
               throw new EventAlreadyExistsException($"Event with Id: {eventItem.Id} already exists in wishlist");
           }
        }

        public async Task<bool> DeleteEventFromWishlistAsync(string UserName, int id)
        {
            var findwishlist = await _repository.GetWishlistByUserNameAsync(UserName);
            if (findwishlist != null)
            {
                var result = await _repository.DeleteEventFromWishlistAsync(UserName, id);
                if (result)
                {
                    return result;
                }
                else
                {
                    throw new EventNotFoundException($"Event with Id: {id} not found in wishlist");
                }
            }
            else
            {
                throw new WishlistNotFoundException($"Wishlist with UserNmae: {UserName} is not created yet");
            }
        }

        public async Task<Wishlist> GetWishlistByUserNameAsync(string UserName)
        {
            var result = await _repository.GetWishlistByUserNameAsync(UserName);
            if (result!=null)
            {
                return result;
            }
            else
            {
                throw new WishlistNotFoundException($"Wishlist with UserNmae: {UserName} not found in wishlist");
            }
        }
    }
}
