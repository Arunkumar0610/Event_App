using Confluent.Kafka;
using Newtonsoft.Json;
using UserProfileService.Models;
namespace UserProfileService.Services
{
    public class KafkaProducerService:IKafkaProducerService
    {
        private readonly ProducerConfig config;
        private readonly ILogger<KafkaProducerService> logger;
        private readonly IConfiguration configuration;
        //kafka connection
        public KafkaProducerService(ProducerConfig _config, IConfiguration _configuration, ILogger<KafkaProducerService> _logger)
        {
            config = _config;
            configuration = _configuration;
            logger = _logger;
        }
        //Logic for sending Userdetails to consumer
        public async Task<bool> SendMessageToKafkaAsync(UserDetails user)
        {
            //converting User object to string
            string serializeddata = JsonConvert.SerializeObject(user).ToString();
            //creating a producer bulider by using which automatically calls Dispose() when using block is exited
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var topic = configuration.GetSection("TopicName").Value;
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = serializeddata });
                    logger.LogInformation("Producer sent userDetails to kafka Consumer successfully");
                    producer.Flush(TimeSpan.FromSeconds(10));
                    return true;
                }
                catch (ProduceException<Null, string> ex)
                {
                    logger.LogError("Error Occured while producer sent userDetails to consumer" + ex.Message);
                }
            }
            logger.LogInformation("Producer failed to sent userDetails to kafka Consumer");
            return false;
        }
    }
}
