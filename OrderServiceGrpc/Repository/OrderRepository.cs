using Dapper;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;
using System.Data.Common;
using System.Transactions;
using Z.Dapper.Plus;

namespace OrderServiceGrpc.Repository
{
    public interface IOrderRepository
    {
        #region Order
        Task<OrderModel> GetOrderById(OrderIdRequest request);
        Task<List<OrderModel>> GetAllOrders(OrderListRequest request);
        Task<bool> AddOrder(OrderModel request, int userId);
        Task<bool> UpdateOrder(OrderModel request, int userId);
        Task<bool> DeleteOrder(int requestId, int userId);
        Task<int> GetOrderCount();
        Task<List<OrderModel>> GetAllOrdersWithPagination(OrderListRequest request);
        #endregion

        #region OrderItems
        Task<List<OrderItemModel>> GetOrderItemsForOrder(int orderId);
        #endregion
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public OrderRepository(IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = _config.GetSection("ConnectionStrings:DefaultConnection").Get<string>() ?? "";
        }

        public async Task<bool> AddOrder(OrderModel request, int userId)
        {
            string sql = @" INSERT INTO [dbo].[Orders]
                                   ([OrderDate]
                                   ,[OrderCounter]
                                   ,[UserId]
                                   ,[Status]
                                   ,[NetAmount]
                                   ,[CreatedBy]
                                   ,[CreatedDate]
                                   ,[IsDeleted])
                             VALUES
                                   (@OrderDate, 
                                   @OrderCounter,
                                   @UserId,
                                   @Status,
                                   @NetAmount,
                                   @CreatedBy, 
                                   @CreatedDate,
                                   @IsDeleted ); select Convert(int,SCOPE_IDENTITY())";

            try
            {
                await using SqlConnection conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                await using DbTransaction t = await conn.BeginTransactionAsync();

                try
                {
                    object[] parameters = { new
                            {
                                OrderDate = request.OrderDate,
                                UserId = request.UserId,
                                Status = request.Status,
                                NetAmount = request.NetAmount,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false
                            }
                        };

                    int insertedOrderId = await conn.ExecuteScalarAsync<int>(sql, parameters);

                    foreach (var item in request.OrderItems)
                    {
                        item.OrderId = insertedOrderId;
                    }

                    await conn.BulkInsertAsync<OrderItemModel>(request.OrderItems);

                    await t.CommitAsync();
                }
                catch (Exception e)
                {
                    await t.RollbackAsync();
                }
                finally
                {
                    await t.DisposeAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Task<bool> DeleteOrder(int request, int userId)
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

        public async Task<bool> UpdateOrder(OrderModel request, int userId)
        {
            const string updateOrderSql = @"
                                            UPDATE Orders SET
                                                OrderDate = @OrderDate,
                                                UserId = @UserId,
                                                Status = @Status,
                                                NetAmount = @NetAmount,
                                                ModifiedBy = @ModifiedBy,
                                                ModifiedDate = @ModifiedDate,
                                                IsDeleted = @IsDeleted
                                            WHERE Id = @Id";

            const string deleteOrderItemsSql = @"DELETE FROM OrderItems WHERE OrderId = @OrderId";

            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                await using var transaction = await conn.BeginTransactionAsync();

                try
                {
                    var parameters = new
                    {
                        request.OrderDate,
                        request.UserId,
                        request.Status,
                        request.NetAmount,
                        ModifiedBy = userId,
                        ModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                        request.Id
                    };

                    await conn.ExecuteAsync(updateOrderSql, parameters, transaction);

                    // Optional: Use proper comparison logic here
                    // Assume we always want to replace items for now
                    await conn.ExecuteAsync(deleteOrderItemsSql, new { OrderId = request.Id }, transaction);

                    // Set the OrderId in each item before inserting
                    foreach (var item in request.OrderItems)
                        item.OrderId = request.Id;

                    var parameters2 = new { };

                    await conn.BulkInsertAsync(request.OrderItems, transaction);

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log ex (optional)
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log ex (optional)
                return false;
            }
        }

        public async Task<List<OrderItemModel>> GetOrderItemsForOrder(int orderId)
        {
            string sql = @"select * from OrderItems where OrderId = @OrderId";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    return (List<OrderItemModel>)await conn.QueryAsync<OrderItemModel>(sql, new { OrderId = orderId });
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
