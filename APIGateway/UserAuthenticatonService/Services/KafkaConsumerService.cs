using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading;
using UserAuthenticatonService.Models;
namespace UserAuthenticatonService.Services
{
    //  KafkaConsumerService as BackgroundService to Consume the messages
    public class KafkaConsumerService : BackgroundService
    {
        //IServiceScopeFactory is to create a scope
        //using serviceProvider to Get the authService
        //and call the authService.RegisterUser() to consume the message
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ConsumerConfig config;
        private readonly ILogger<KafkaConsumerService> logger;
        private readonly IConfiguration configuration;
        public KafkaConsumerService(IServiceScopeFactory _serviceScopeFactory, ConsumerConfig _config,
            ILogger<KafkaConsumerService>_logger, IConfiguration _configuration)
        {
            serviceScopeFactory = _serviceScopeFactory;
            config = _config;
            logger = _logger;
            configuration=_configuration;
        }
        //overriding async task ExecuteAsync for getting message from kafka producer to consumer and store it in database
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var authService = scope.ServiceProvider.GetService<IAuthService>();
                using (var builder = new ConsumerBuilder<Null, string>(config).Build())
                {
                    var topic = configuration.GetSection("TopicName").Value;
                    //Subscribing topic
                    builder.Subscribe(topic);
                        logger.LogInformation($"kafka Consumer is running in background on topic= {topic}");
                        try
                        {
                        //running a task its wait till task completed
                        await Task.Run(async() =>
                        {
                            logger.LogInformation($"kafka Consumer started recieving messages from kafka producer");
                            while (true)
                            {
                                //consumerResult consuming a cancellation token
                                var consumer = builder.Consume(stoppingToken);
                                //converting message to userDetails Object
                                var user = JsonConvert.DeserializeObject<User>(consumer.Message.Value);
                                if (user?.UserName != null)
                                {
                                    logger.LogInformation($"Message is sent to store in database");
                                    //Converted userDetails object is added to database
                                    var flag = await authService.RegisterUser(user);
                                    if (flag)
                                    {
                                        logger.LogInformation("Message Processed");
                                    }
                                    else
                                    {
                                        logger.LogError("Message was not inserted");
                                    }
                                    break;
                                }
                            }
                        },stoppingToken);
                        await Task.Delay(1000, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);
                        builder.Close();
                    }
                }
            }
        }
    } 
}