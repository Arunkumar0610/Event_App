using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WishlistService.Models
{
    public class Wishlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string WishlistId { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public required string UserName { get; set; }

        public List<EventItem> Events { get; set; }
    }
}
