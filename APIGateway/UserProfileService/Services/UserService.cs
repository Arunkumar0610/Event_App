using System.ComponentModel;
using Confluent.Kafka;
using UserProfileService.Exceptions;
using UserProfileService.Models;
using UserProfileService.Repositories;
using Newtonsoft.Json;
namespace UserProfileService.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IKafkaProducerService kafkaProducer;
        
        public UserService(IUserRepository _userRepository, IKafkaProducerService _kafkaProducer)
        {
            userRepository = _userRepository;
            kafkaProducer=_kafkaProducer;
        }
        public async Task<Users> RegisterUser(Users user) 
        { 
           var userfound= await userRepository.GetUserByUserName(user.UserName);
            if(userfound == null)
            {
                UserDetails userDetails = new () 
                { 
                    UserName = user.UserName,
                    Email=user.Email,
                    Password=user.Password
                };
                var flag = await kafkaProducer.SendMessageToKafkaAsync(userDetails);
                if (flag)
                {
                    return await userRepository.RegisterUser(user);
                }
                else
                {
                    throw new KafkaProducerException("Some error occured not able to sent a message to consumer");
                }
            }
            else
            {
                throw new UserAlreadyExistException($"User with {user.UserName} already exists");
            }
        }
        public async Task<Users> GetUserById(string id) 
        {
            var user= await userRepository.GetUserById(id);
            if(user!=null)
            {
                return user;
            }
            else
            {
                throw new UserNotFoundException($"User with {id} not found");
            }
        }
        public async Task<Users> GetUserByUserName(string userName) 
        {
            var user = await userRepository.GetUserByUserName(userName);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new UserNotFoundException($"User with {userName} not found");
            }
        }
    }
}
