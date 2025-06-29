using Dapper;
using Microsoft.Data.SqlClient;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;

namespace OrderServiceGrpc.Repository
{
    public interface ICustomerTransactionRepository
    {
        Task<List<CustomerTransactionModel>> GetTransactionById(string connectionString, TransactionRequest request);
        Task<List<CustomerTransactionModel>> GetAllTransactions(string connectionString, TransactionRequest request);
        Task<bool> AddTransaction(string connectionString, TransactionObject request, int userId);
        Task<bool> UpdateTransaction(string connectionString, TransactionObject request, int userId);
        Task<bool> DeleteTransaction(string connectionString, TransactionObject request, int userId);
        Task<int> GetTransactionCount(string connectionString);
    }

    public class CustomerTransactionRepository : ICustomerTransactionRepository
    {
        public Task<bool> AddTransaction(string connectionString, TransactionObject request, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTransaction(string connectionString, TransactionObject request, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomerTransactionModel>> GetAllTransactions(string connectionString, TransactionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomerTransactionModel>> GetTransactionById(string connectionString, TransactionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTransactionCount(string connectionString)
        {
            try
            {
                using (var db = new SqlConnection(connectionString))
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

        public Task<bool> UpdateTransaction(string connectionString, TransactionObject request, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
