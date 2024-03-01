using UserProfileService.Models;
namespace UserProfileService.Services
{
    public interface IUserService
    {
        public Task<Users> RegisterUser(Users user);
        public Task<Users> GetUserById(string id);
        public Task<Users> GetUserByUserName(string userName);
    }
}
