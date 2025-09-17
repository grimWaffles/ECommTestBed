using API_Gateway.Models;
using Confluent.Kafka;
using System.Text.Json;

namespace API_Gateway.Services
{
    public interface IOrderEventProducer
    {
        bool PublishOrderEvent();
    }

    public class OrderEventProducer : IOrderEventProducer
    {
        private readonly string _bootstrapServer;
        private readonly IConfiguration _config;
        private readonly ProducerConfig _producerConfig;

        public OrderEventProducer(IConfiguration configuration)
        {
            _config = configuration;
            _bootstrapServer = _config["Kafka:BootstrapServer"] ?? "";

            _producerConfig = new ProducerConfig { BootstrapServers = _bootstrapServer };
        }

        public bool PublishOrderEvent()
        {
            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                var orderEvent = new OrderCreatedEvent()
                {
                    OrderId = 1,
                    CustomerId = 123,
                    Amount = 250.75m,
                    CreatedAt = DateTime.UtcNow
                };

                string eventJson = JsonSerializer.Serialize(orderEvent);

                for(int i = 0; i < 500; i++)
                {
                    try
                    {
                        producer.ProduceAsync("order-create", new Message<string, string> { Key = orderEvent.OrderId.ToString(), Value = eventJson });
                    }
                    catch (Exception e)
                    {
                        return false;
                    }

                    Task.Delay(100);
                }
            }
            return true;
        }
    }
}
