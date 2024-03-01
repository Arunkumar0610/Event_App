using System.ComponentModel.DataAnnotations;
namespace UserProfileService.Models
{
    public class UserDetails
    {
        [Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is Invalid")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+=;:<>|./?,-]).{8,15}$",
            ErrorMessage = "Password must be between 8 and 15 characters and contain atleast one uppercase," +
            "lowercase,number and special character.")]
        public required string Password { get; set; }
    }
}
