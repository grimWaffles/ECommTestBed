using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;

namespace OrderServiceGrpc.Repository
{
    public interface IOrderRepository
    {
        Task<OrderModel> GetOrderById(OrderIdRequest request);
        Task<List<OrderModel>> GetAllOrders(OrderListRequest request);
        Task<bool> AddOrder(CreateOrderRequest request, int userId);
        Task<bool> UpdateOrder(UpdateOrderRequest request, int userId);
        Task<bool> DeleteOrder(DeleteOrderRequest request, int userId);
        Task<int> GetOrderCount();
        Task<List<OrderModel>> GetAllOrdersWithPagination(OrderListRequest request);
    }

    public class OrderRepository : IOrderRepository
    {
        public Task<bool> AddOrder(CreateOrderRequest request, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrder(DeleteOrderRequest request, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderModel>> GetAllOrders(OrderListRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderModel>> GetAllOrdersWithPagination(OrderListRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrderModel> GetOrderById(OrderIdRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOrderCount()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrder(UpdateOrderRequest request, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
