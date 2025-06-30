using Grpc.Core;
using OrderServiceGrpc.Protos;
using OrderServiceGrpc.Repository;

namespace OrderServiceGrpc.Services
{
    public class CustomerTransactionService : CustomerTransaction.CustomerTransactionBase
    {
        private readonly ICustomerTransactionRepository _trxRepo;

        public CustomerTransactionService(IConfiguration configuration, ICustomerTransactionRepository customerTransactionRepository)
        {
            _trxRepo = customerTransactionRepository;
        }

        public override Task<TransactionResponse> GetAllTransactions(TransactionRequest request, ServerCallContext context)
        {
            return base.GetAllTransactions(request, context);
        }

        public override Task<TransactionResponse> GetTransactionById(TransactionRequest request, ServerCallContext context)
        {
            return base.GetTransactionById(request, context);
        }

        public override Task<TransactionResponse> AddTransaction(TransactionObject request, ServerCallContext context)
        {
            return base.AddTransaction(request, context);
        }

        public override Task<TransactionResponse> UpdateTransaction(TransactionObject request, ServerCallContext context)
        {
            return base.UpdateTransaction(request, context);
        }

        public override Task<TransactionResponse> DeleteTransaction(TransactionObject request, ServerCallContext context)
        {
            return base.DeleteTransaction(request, context);
        }
    }
}
