using Dapper;
using Microsoft.Data.SqlClient;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;
using static Dapper.SqlMapper;

namespace OrderServiceGrpc.Repository
{
    public interface ICustomerTransactionRepository
    {
        Task<CustomerTransactionModel> GetTransactionById(TransactionRequest request);
        Task<List<CustomerTransactionModel>> GetAllTransactions(TransactionRequest request);
        Task<bool> AddTransaction(TransactionObject request, int userId);
        Task<bool> UpdateTransaction(TransactionObject request, int userId);
        Task<bool> DeleteTransaction(TransactionObject request, int userId);
        Task<int> GetTransactionCount();
        Task<List<CustomerTransactionModel>> GetAllTransactionsWithPagination(TransactionRequest request);
    }

    public class CustomerTransactionRepository : ICustomerTransactionRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        public CustomerTransactionRepository(IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = _config.GetSection("ConnectionString:DefaultConnection").Get<string>() ?? "";
        }
        public Task<bool> AddTransaction(TransactionObject request, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTransaction(TransactionObject request, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CustomerTransactionModel>> GetAllTransactions(TransactionRequest request)
        {
            try
            {
                string sql = @"select * from Transactions";

                SqlConnection conn = new SqlConnection(_connectionString);

                //List<CustomerTransactionModel> list = new List<CustomerTransactionModel>();

                List<CustomerTransactionModel> transactions = (List<CustomerTransactionModel>)await conn.QueryAsync<CustomerTransactionModel>(sql);

                //foreach(var transaction in transactions)
                //{
                //    list.Add(transaction);
                //}
                return transactions;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<CustomerTransactionModel>> GetAllTransactionsWithPagination(TransactionRequest request)
        {
            try
            {
                string sql = @"	SELECT 
		                            [Id]
		                            ,[UserId]
		                            ,[TransactionType]
		                            ,[Amount]
		                            ,[CreatedBy]
		                            ,[CreatedDate]
		                            ,[ModifiedDate]
		                            ,[ModifiedBy]
		                            ,[IsDeleted]
		                            ,[TransactionDate]
	                            FROM [ECommercePlatform].[dbo].[CustomerTransactions]
	                            WHERE Convert(date,TransactionDate) between @StartDate and @EndDate
	                            ORDER BY Id
	                            OFFSET (@PageNumber-1)*(@PageSize) ROWS
	                            FETCH NEXT @PageSize ROWS ONLY

	                            SELECT COUNT(*) TotalTransactions FROM CustomerTransactions";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    GridReader resultSet = await conn.QueryMultipleAsync(sql);

                    List<CustomerTransactionModel> list = (List<CustomerTransactionModel>)await resultSet.ReadAsync<CustomerTransactionModel>();
                    int totalRows = await resultSet.ReadSingleAsync<int>();

                    return list;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CustomerTransactionModel> GetTransactionById(TransactionRequest request)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    string sql = @" select 
                                        *--TransactionType, Amount, TransactionDate, UserId 
                                    from CustomerTransactions 
                                    where Id = @Id";

                    CustomerTransactionModel model = await db.QuerySingleAsync<CustomerTransactionModel>(sql, new { Id = 1 });

                    await db.DisposeAsync();

                    return model;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> GetTransactionCount()
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    string sql = "select count(*) from CustomerTransactions";

                    return await db.ExecuteScalarAsync<int>(sql);
                }

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public Task<bool> UpdateTransaction(TransactionObject request, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
