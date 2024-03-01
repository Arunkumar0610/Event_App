using UserProfileService.Models;
namespace UserProfileService.Repositories
{
    public interface IUserRepository
    {
        public  Task<Users> RegisterUser(Users user);
        public   Task<Users> GetUserById(string id);
        public   Task<Users> GetUserByUserName(string userName);
    }
}
