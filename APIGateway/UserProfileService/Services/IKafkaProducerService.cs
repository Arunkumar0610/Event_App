using UserProfileService.Models;

namespace UserProfileService.Services
{
    public interface IKafkaProducerService
    {
        public  Task<bool> SendMessageToKafkaAsync(UserDetails user);
    }
}
