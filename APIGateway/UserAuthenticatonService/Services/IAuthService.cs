using UserAuthenticatonService.Models;
namespace UserAuthenticatonService.Services
{
    public interface IAuthService
    {
        public Task<LoginResponse> Login(Login login);
        public Task<bool> RegisterUser(User user);
        //public void GetDataFromKafkaAsync();
    }
}
