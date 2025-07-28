using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using OrderServiceGrpc.Models;
using OrderServiceGrpc.Protos;
using OrderServiceGrpc.Repository;

namespace OrderServiceGrpc.Services
{
    public class CustomerTransactionGrpcService : CustomerTransactionService.CustomerTransactionServiceBase
    {
        private readonly ICustomerTransactionRepository _repo;

        public CustomerTransactionGrpcService(ICustomerTransactionRepository repo)
        {
            _repo = repo;
        }

        public override async Task<TransactionResponseSingle> GetTransactionById(TransactionRequestSingle request, ServerCallContext context)
        {
            var model = await _repo.GetTransactionById(request.Id);
            if (model == null)
            {
                return new TransactionResponseSingle
                {
                    Status = 1,
                    ErrorMessage = "Transaction not found"
                };
            }

            return new TransactionResponseSingle
            {
                Status = 0,
                Dto = MapToDto(model)
            };
        }

        public override async Task<TransactionResponseMultiple> GetAllTransactions(TransactionRequestMultiple request, ServerCallContext context)
        {
            var models = await _repo.GetAllTransactionsWithPagination(request);

            var response = new TransactionResponseMultiple
            {
                Status = models != null,
                ErrorMessage = models == null ? "Error fetching transactions" : string.Empty
            };

            if (models != null)
            {
                response.Transactions.AddRange(models.Select(MapToDto));
            }

            return response;
        }

        public override async Task<TransactionCrudResponse> AddTransaction(TransactionDto request, ServerCallContext context)
        {
            var result = await _repo.AddTransaction(request, (int)request.CreatedBy);

            return new TransactionCrudResponse
            {
                Status = result ? 0 : 1,
                ErrorMessage = result ? "" : "Failed to add transaction"
            };
        }

        public override async Task<TransactionCrudResponse> UpdateTransaction(TransactionDto request, ServerCallContext context)
        {
            var result = await _repo.UpdateTransaction(request, (int)request.ModifiedBy);

            return new TransactionCrudResponse
            {
                Status = result ? 0 : 1,
                ErrorMessage = result ? "" : "Failed to update transaction"
            };
        }

        public override async Task<TransactionCrudResponse> DeleteTransaction(TransactionRequestSingle request, ServerCallContext context)
        {
            // We’ll create a DTO from just the ID
            var dto = new TransactionDto
            {
                Id = request.Id
            };

            var result = await _repo.DeleteTransaction(dto, request.UserId);

            return new TransactionCrudResponse
            {
                Status = result ? 0 : 1,
                ErrorMessage = result ? "" : "Failed to delete transaction"
            };
        }

        // ---------------------------
        // 🔁 Mapping Helpers
        // ---------------------------
        private TransactionDto MapToDto(CustomerTransactionModel model)
        {
            return new TransactionDto
            {
                Id = model.Id,
                UserId = model.UserId,
                TransactionType = model.TransactionType,
                Amount = (double)model.Amount,
                CreatedBy = model.CreatedBy,
                CreatedDate = Timestamp.FromDateTime(model.CreatedDate.ToUniversalTime()),
                ModifiedBy = model.ModifiedBy,
                TransactionDate = Timestamp.FromDateTime(model.TransactionDate.ToUniversalTime()),
                IsDeleted = model.IsDeleted
            };
        }
    }
}
