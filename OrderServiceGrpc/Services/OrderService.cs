
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrderServiceGrpc.Protos;

namespace OrderServiceGrpc.Services
{
    public class OrderService : OrderGrpcService.OrderGrpcServiceBase
    {
        public override Task<OrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            return base.CreateOrder(request, context);
        }

        public override Task<OrderResponse> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
        {
            return base.DeleteOrder(request, context);
        }
        public override Task<OrderListResponse> GetAllOrders(Empty request, ServerCallContext context)
        {
            return base.GetAllOrders(request, context);
        }

        public override Task<OrderResponse> GetOrderById(OrderIdRequest request, ServerCallContext context)
        {
            return base.GetOrderById(request, context);
        }

        public override Task<OrderListResponse> GetOrdersByUser(UserIdRequest request, ServerCallContext context)
        {
            return base.GetOrdersByUser(request, context);
        }

        public override Task<OrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
        {
            return base.UpdateOrder(request, context);
        }
    }
}
