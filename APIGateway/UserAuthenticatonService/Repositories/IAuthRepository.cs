using UserAuthenticatonService.Models;
namespace UserAuthenticatonService.Repositories
{
    public interface IAuthRepository
    {
        public Task<LoginResponse> Login(Login login);
        public Task<Response> RegisterUser (User user);
    }
}
