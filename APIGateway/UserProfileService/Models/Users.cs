using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace UserProfileService.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string UserId { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(25,ErrorMessage = "Name length should be maximum 25 characters")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+=;:<>|./?,-]).{8,15}$",
            ErrorMessage = "Password must be between 8 and 15 characters and contain atleast one uppercase," +
            "lowercase,number and special character.")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = " Password and ConfirmPassword should match")]
        public required string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Email is Invalid")]
        public required string Email { get; set;}
        [Required(ErrorMessage = "Contact is required")]
        [Phone]
        public required string Contact { get; set; }
        }
}
