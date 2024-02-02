using KafkaNet.Protocol;
using KafkaNet;
using KafkaNet.Model;
using Microsoft.Extensions.Configuration;
using NotificationService.Models.Entities;
using Newtonsoft.Json;

namespace NotificationService.Models
{
    public class ProducerService
    {
        private readonly IBrokerRouter _brokerRouter;
        private readonly IConfiguration _configuration;

        public ProducerService(IConfiguration configuration)
        {
            var kafkaOptions = new KafkaOptions(new Uri(configuration.GetValue<string>("Kafka:Url")));
            _brokerRouter = new BrokerRouter(kafkaOptions);
            _configuration = configuration;
        }

        public async Task SendMessageAsync(Email model)
        {
            var producer = new Producer(_brokerRouter);
            string topic = _configuration.GetValue<string>("Kafka:Topic");
            string message = JsonConvert.SerializeObject(model);

            await producer.SendMessageAsync(topic, new[] { new Message(message), });
        }
    }
}
