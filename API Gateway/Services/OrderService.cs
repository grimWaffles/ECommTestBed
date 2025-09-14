using ApiGateway.Protos;
using Grpc.Net.Client;
namespace API_Gateway.Services
{
    public interface IOrderGrpcClient
    {
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderResponse> GetOrderByIdAsync(OrderIdRequest request);
        Task<OrderListResponse> GetOrdersByUserAsync(UserIdRequest request);
        Task<OrderListResponse> GetAllOrdersAsync(OrderListRequest request);
        Task<OrderResponse> UpdateOrderAsync(UpdateOrderRequest request);
        Task<OrderResponse> DeleteOrderAsync(DeleteOrderRequest request);
    }
    public class OrderGrpcClient : IOrderGrpcClient
    {
        private readonly OrderGrpcService.OrderGrpcServiceClient _client;

        public OrderGrpcClient(IConfiguration configuration)
        {
            string serviceUrl = configuration["Microservices:orderService"];
            if (string.IsNullOrEmpty(serviceUrl))
            {
                throw new ArgumentException("gRPC service URL not configured in appsettings.json");
            }

            var httpHandler = new HttpClientHandler
            {
                // This is optional and should be used only in development for insecure certs
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel = GrpcChannel.ForAddress(serviceUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _client = new OrderGrpcService.OrderGrpcServiceClient(channel);
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
            => await _client.CreateOrderAsync(request);

        public async Task<OrderResponse> GetOrderByIdAsync(OrderIdRequest request)
            => await _client.GetOrderByIdAsync(request);

        public async Task<OrderListResponse> GetOrdersByUserAsync(UserIdRequest request)
            => await _client.GetOrdersByUserAsync(request);

        public async Task<OrderListResponse> GetAllOrdersAsync(OrderListRequest request)
            => await _client.GetAllOrdersAsync(request);

        public async Task<OrderResponse> UpdateOrderAsync(UpdateOrderRequest request)
            => await _client.UpdateOrderAsync(request);

        public async Task<OrderResponse> DeleteOrderAsync(DeleteOrderRequest request)
            => await _client.DeleteOrderAsync(request);
    }
}
