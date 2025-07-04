using Dapper;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;
using System.Data.Common;
using System.Diagnostics;
using System.Transactions;
using Z.Dapper.Plus;
using static Dapper.SqlMapper;

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
        Task<Tuple<int, List<OrderModel>>> GetAllOrdersWithPagination(OrderListRequest request);
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
            string insertOrderSql = @" INSERT INTO [dbo].[Orders]
                                   ([OrderDate] ,[OrderCounter] ,[UserId]  ,[Status] ,[NetAmount] ,[CreatedBy] ,[CreatedDate] ,[IsDeleted])
                             VALUES (@OrderDate,  @OrderCounter, @UserId, @Status, @NetAmount, @CreatedBy,  @CreatedDate, @IsDeleted ); select Convert(int,SCOPE_IDENTITY())";

            string insertOrderItemsSql = @" Insert into OrderItems(OrderId, ProductId, Quantity, GrossAmount, Status, CreatedBy, CreatedDate, IsDeleted)
                                            values (@OrderId, @ProductId, @Quantity, @GrossAmount, @Status, @CreatedBy, @CreatedDate, @IsDeleted)";

            try
            {
                await using SqlConnection conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                await using DbTransaction t = await conn.BeginTransactionAsync();

                try
                {
                    var insertOrderParameters = new
                    {
                        OrderDate = request.OrderDate,
                        UserId = request.UserId,
                        Status = request.Status,
                        NetAmount = request.NetAmount,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    int insertedOrderId = await conn.ExecuteScalarAsync<int>(insertOrderSql, insertOrderParameters, t);

                    foreach (var item in request.OrderItems)
                    {
                        var parameters = new
                        {
                            OrderId = insertedOrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quatity,
                            GrossAmount = item.GrossAmount,
                            Status = item.Status,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false
                        };

                        await conn.ExecuteAsync(insertOrderItemsSql, parameters, t);
                    }

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

        public async Task<bool> DeleteOrder(int request, int userId)
        {
            const string deleteOrderItemsSql = @"update OrderItems set IsDeleted = 1, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate where OrderId = @OrderId";
            const string deleteOrderSql = @"update Orders set IsDeleted = 1, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate where Id = @OrderId";

            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new
            {
                ModifiedBy = userId,
                ModifiedDate = DateTime.Now
            };

            await using DbTransaction transaction = await conn.BeginTransactionAsync();

            try
            {
                await conn.ExecuteAsync(deleteOrderItemsSql, parameters, transaction);
                await conn.ExecuteAsync(deleteOrderSql, parameters, transaction);

                await transaction.CommitAsync();
            }
            catch(Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }

            return true;
        }

        public async Task<List<OrderModel>> GetAllOrders(OrderListRequest request)
        {
            string sql = @"select * from Orders order by OrderDate desc";
            try
            {
                await using SqlConnection conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                return (List<OrderModel>)await conn.QueryAsync<OrderModel>(sql);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<int,List<OrderModel>>> GetAllOrdersWithPagination(OrderListRequest request)
        {
            const string sql = @"   declare @TotalOrders int, @TotalPages int
                                    select
	                                    o.*
                                    from Orders o
                                    order by o.OrderDate desc
                                    offset (@pageSize-1)*@PageNumber rows
                                    fetch next @PageSize rows only

                                    select @TotalOrders = count(*) from Orders
                                    select @TotalPages = (@TotalOrders/@PageSize) + 1

                                    select @TotalPages TotalPages";
            try
            {
                await using SqlConnection conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var parameters = new
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                GridReader reader = await conn.QueryMultipleAsync(sql, parameters);

                List<OrderModel> orders = reader.Read<OrderModel>().ToList();
                int totalPages = reader.ReadSingle<int>();

                return Tuple.Create<int, List<OrderModel>>(totalPages,orders);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<OrderModel> GetOrderById(OrderIdRequest request)
        {
            const string fetchOrderSql = @"select * from Orders where Id = @OrderId;
                                            select * from OrderItems where OrderId = @OrderId and IsDeleted = 0;";
            
            try
            {
                await using SqlConnection conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var parameters = new { OrderId = request.Id };

                GridReader reader = await conn.QueryMultipleAsync(fetchOrderSql, parameters);

                OrderModel model = reader.Read<OrderModel>().First();

                model.OrderItems = reader.Read<OrderItemModel>().ToList();

                return model;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<int> GetOrderCount()
        {
            string sql = @"select Count(*) from Orders";
            try
            {
                await using SqlConnection conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                return await conn.ExecuteScalarAsync<int>(sql);
            }
            catch(Exception e)
            {
                return 0;
            }
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

            const string deleteOrderItemsSql = @"update OrderItems set IsDeleted = 1, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate where OrderId = @OrderId";
            const string insertOrderItemsSql = @" Insert into OrderItems(OrderId, ProductId, Quantity, GrossAmount, Status, CreatedBy, CreatedDate, IsDeleted)
                                            values (@OrderId, @ProductId, @Quantity, @GrossAmount, @Status, @CreatedBy, @CreatedDate, @IsDeleted)";
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                await using var transaction = await conn.BeginTransactionAsync();

                try
                {
                    var parameters = new
                    {
                        OrderDate= request.OrderDate,
                        UserId=request.UserId,
                        Status =request.Status,
                        NetAmount = request.NetAmount,
                        ModifiedBy = userId,
                        ModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                        request.Id
                    };

                    await conn.ExecuteAsync(updateOrderSql, parameters, transaction);
                    await conn.ExecuteAsync(deleteOrderItemsSql, new { OrderId = request.Id }, transaction);

                    foreach (var item in request.OrderItems)
                    {
                        item.OrderId = request.Id;

                        var orderItemParams = new
                        {
                            OrderId = request.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quatity,
                            GrossAmount = item.GrossAmount,
                            Status = item.Status,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false
                        };

                        await conn.ExecuteAsync(insertOrderItemsSql,item, transaction);
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
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
