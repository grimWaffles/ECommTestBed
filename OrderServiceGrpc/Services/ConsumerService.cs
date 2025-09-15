
using Confluent.Kafka;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Repository;
using System.Diagnostics;
using System.Text.Json;

namespace OrderServiceGrpc.Services
{
    public class ConsumerBackgroundService : BackgroundService
    {
        private readonly string _bootstrapServer;
        private readonly string _topic;
        private readonly string _groupId;
        private readonly IOrderRepository _orderRepository;

        public ConsumerBackgroundService(IConfiguration configuration, IOrderRepository orderRepository)
        {
            _bootstrapServer = configuration["Kafka:BootstrapServer"] ?? "";
            _topic = configuration["Kafka:Topic"] ?? "";
            _groupId = configuration["Kafka:GroupId"] ?? "";

            _orderRepository = orderRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumerConfig config = new ConsumerConfig()
            {
                BootstrapServers = _bootstrapServer,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        OrderCreatedEvent model = JsonSerializer.Deserialize<OrderCreatedEvent>(result.Message.Value);
                        
                        if(result != null)
                        {
                            await _orderRepository.InsertOrderCreateEvent(Convert.ToInt32(result.Message.Key));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
