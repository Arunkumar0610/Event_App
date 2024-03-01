using MongoDB.Driver;
using WishlistService.Database;
using WishlistService.Models;

namespace WishlistService.Repositories
{
    public class WishlistRepository:IWishlistRepository
    {
        private readonly IMongoCollection<Wishlist> Wishlists;
        public WishlistRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Wishlists = database.GetCollection<Wishlist>(settings.CollectionName);
        }

        public async Task<EventItem> AddEventToWishlistAsync(string UserName,EventItem eventItem)
        {
            var findwishlist = await Wishlists.Find(x => x.UserName == UserName).FirstOrDefaultAsync();
            //var countWishlist= await Wishlists.CountDocumentsAsync(s=>s.UserName==UserName);
            if (findwishlist == null)
            {
                Wishlist item = new Wishlist()
                {
                    WishlistId = "",
                    UserName = UserName,
                    Events=new List<EventItem>(){eventItem}
                };
                await Wishlists.InsertOneAsync(item);
                return eventItem;
            }
            else
            {
                //var findwishlist = await Wishlists.Find(x => x.UserName == UserName).FirstOrDefaultAsync();
                var eventexists = findwishlist.Events.FirstOrDefault(x => x.Id == eventItem.Id);
                if (eventexists == null)
                {
                    findwishlist.Events.Add(eventItem);
                    var updatedefination = Builders<Wishlist>.Update
                        .Set(x => x.Events, findwishlist.Events);

                   await Wishlists.UpdateOneAsync(x => x.UserName == UserName, updatedefination);
                    return eventItem;
                }

                return null;
            }
        }

        public async Task<bool> DeleteEventFromWishlistAsync(string UserName,int id)
        {
            var findwishlist = await Wishlists.Find(x => x.UserName == UserName).FirstOrDefaultAsync();
           
            var eventexists = findwishlist.Events.FirstOrDefault(x => x.Id ==id);
            if (eventexists != null)
            {
                findwishlist.Events.Remove(eventexists);
                var updatedefination = Builders<Wishlist>.Update
                    .Set(x => x.Events, findwishlist.Events);

                await Wishlists.UpdateOneAsync(x => x.UserName == UserName, updatedefination);
                return true;
            }

            return false;
        }

        public async Task<Wishlist> GetWishlistByUserNameAsync(string UserName)
        {
            var wishlistEvents =await Wishlists.Find(x => x.UserName == UserName).FirstOrDefaultAsync();
            return wishlistEvents;
        }
    }
}
