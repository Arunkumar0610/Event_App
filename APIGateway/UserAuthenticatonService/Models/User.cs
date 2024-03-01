using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace UserAuthenticatonService.Models
{
    public class User
    {
        [Required(ErrorMessage ="User Name is required")]
        public  required string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public required string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public required string Password { get; set; }    
    }
}
