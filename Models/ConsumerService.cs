using KafkaNet;
using KafkaNet.Model;
using Newtonsoft.Json;
using NotificationService.Models.Entities;
using System.Text;
using System.Text.Json.Serialization;

namespace NotificationService.Models
{
    public class ConsumerService : BackgroundService
    {
        private readonly IBrokerRouter _brokerRouter;
        private readonly IConfiguration _configuration;

        private readonly NotificationMail _NotiificationMail;

        public ConsumerService(IConfiguration configuration)
        {
            var kafkaOptions = new KafkaOptions(new Uri(configuration.GetValue<string>("Kafka:Url")));
            _brokerRouter = new BrokerRouter(kafkaOptions);
            _configuration = configuration;

            _NotiificationMail = new NotificationMail(configuration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                StartConsume(stoppingToken);

                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            return;
        }

        public void StartConsume(CancellationToken stoppingToken)
        {
            var consumer = new Consumer(new ConsumerOptions(_configuration.GetValue<string>("Kafka:Topic"), _brokerRouter));

            foreach (var message in consumer.Consume())
            {
                if (message == null)
                    continue;

                var val = Encoding.UTF8.GetString(message.Value);
                var model = JsonConvert.DeserializeObject<Email>(val);

                _NotiificationMail.SendMail(model);
            }
        }
    }
}
