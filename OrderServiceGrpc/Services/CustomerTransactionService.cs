using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using OrderServiceGrpc.Models;
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

        public override async Task<TransactionResponse> GetAllTransactions(TransactionRequest request, ServerCallContext context)
        {
            List<CustomerTransactionModel> list = await _trxRepo.GetAllTransactionsWithPagination(request);
            List<TransactionObject> result = new List<TransactionObject>();

            if (list.IsNullOrEmpty())
            {
                return new TransactionResponse()
                {
                    Status = false,
                    ErrorMessage = "No data found",
                    ListOfTransactions = { new List<TransactionObject>() }
                };
            }

            List<TransactionObject> r = (List<TransactionObject>)list.Select(MapToResponse);

            return new TransactionResponse()
            {
                Status = true,
                ErrorMessage = "",
                ListOfTransactions = { result }
            };
        }

        public override async Task<TransactionResponse> GetTransactionById(TransactionRequest request, ServerCallContext context)
        {
            CustomerTransactionModel model = await _trxRepo.GetTransactionById(request);
            
            List<TransactionObject> result = new List<TransactionObject>();

            if (model==null)
            {
                return new TransactionResponse()
                {
                    Status = false,
                    ErrorMessage = "No data found",
                    ListOfTransactions = { new List<TransactionObject>() }
                };
            }

            List<TransactionObject> list = new List<TransactionObject>() { MapToResponse(model)};

            return new TransactionResponse()
            {
                Status = true,
                ErrorMessage = "",
                ListOfTransactions = { result }
            };
        }

        public override async Task<TransactionResponse> AddTransaction(TransactionObject request, ServerCallContext context)
        {
            int userId = 1;
            bool result = await _trxRepo.AddTransaction(request,userId);

            return new TransactionResponse()
            {
                Status = result,
                ErrorMessage = result ? "Saved successfully" : "Failed to save record"
            };
        }

        public override async Task<TransactionResponse> UpdateTransaction(TransactionObject request, ServerCallContext context)
        {
            int userId = 1;
            bool result = await _trxRepo.UpdateTransaction(request, userId);

            return new TransactionResponse()
            {
                Status = result,
                ErrorMessage = result ? "Updated successfully" : "Failed to update record"
            };
        }

        public override async Task<TransactionResponse> DeleteTransaction(TransactionObject request, ServerCallContext context)
        {
            int userId = 1;
            bool result = await _trxRepo.DeleteTransaction(request, userId);

            return new TransactionResponse()
            {
                Status = result,
                ErrorMessage = result ? "Deleted successfully" : "Failed to delete record"
            };
        }

        private static TransactionObject MapToResponse(CustomerTransactionModel model)
        {
            return new TransactionObject()
            {
                Id = model.Id,
                UserId = model.UserId,
                TransactionType = model.TransactionType,
                Amount = (double)model.Amount,
                CreatedBy = model.CreatedBy,
                CreatedDate = Timestamp.FromDateTimeOffset(model.CreatedDate),
                IsDeleted = model.IsDeleted,
                TransactionDate = Timestamp.FromDateTimeOffset(model.TransactionDate)
            };
        }
    }
}
