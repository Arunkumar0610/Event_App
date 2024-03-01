using Confluent.Kafka;
using Newtonsoft.Json;
using UserAuthenticatonService.Exceptions;
using UserAuthenticatonService.Models;
using UserAuthenticatonService.Repositories;
namespace UserAuthenticatonService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;      
        public AuthService(IAuthRepository authRepository)
        { 
            _authRepository = authRepository;          
        }
        public async Task<LoginResponse> Login(Login login)
        {
            var result= await _authRepository.Login(login);
            if(result.Token!=null)
            {
                return result;
            }
            else
            {
                throw new InvalidUserCredentialsException("UserName or Password are Invalid");
            }
        }
        public async Task<bool> RegisterUser(User user)
        { 
            var result=await _authRepository.RegisterUser(user);           
            if (result.Status == "Success")
            { return true; }
            else
            { return false; }
        }
    }
}
