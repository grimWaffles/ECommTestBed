using EfCoreTutorial.Entity.ECommerceModels;
using Google.Protobuf;
using Grpc.Core;
using ProductServiceGrpc.Database;
using ProductServiceGrpc.Repository;

namespace ProductServiceGrpc.Services
{
    public class SellerService : Seller.SellerBase
    {
        private readonly ISellerRepository _repo;

        public SellerService(ISellerRepository repo)
        {
            _repo = repo;
        }

        public override async Task<SellerResponse> CreateSeller(SellerCreateRequest request, ServerCallContext context)
        {
            SellerModel seller = new SellerModel
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                MobileNo = request.MobileNo,
                Email = request.Email,
                Rating = (decimal)request.Rating,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                await _repo.CreateSellerAsync(seller);
            }
            catch(Exception e)
            {
                return null;
            }

            return MapToResponse(seller);
        }

        public override async Task<SellerResponse> GetSellerById(SellerRequest request, ServerCallContext context)
        {
            SellerModel seller = await _repo.GetSellerByIdAsync(request.Id);

            return seller == null ? null : MapToResponse(seller);
        }

        public override async Task<SellerListResponse> GetAllSellers(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var sellers = await _repo.GetAllSellersAsync();

            var response = new SellerListResponse();
            response.Sellers.AddRange(sellers.Select(MapToResponse));
            return response;
        }

        public override async Task<SellerResponse> UpdateSeller(SellerUpdateRequest request, ServerCallContext context)
        {
            var seller = await _repo.GetSellerByIdAsync(request.Id);

            if (seller == null || seller.IsDeleted) return null;

            await _repo.UpdateSellerAsync(seller);

            return MapToResponse(seller);
        }

        public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteSeller(SellerDeleteRequest request, ServerCallContext context)
        {
            var seller = await _repo.GetSellerByIdAsync(request.Id);

            if (seller != null && !seller.IsDeleted)
            {
                await _repo.DeleteSellerAsync(request.Id, request.ModifiedBy);
            }

            return new Google.Protobuf.WellKnownTypes.Empty();
        }

        private SellerResponse MapToResponse(SellerModel seller)
        {
            return new SellerResponse
            {
                Id = seller.Id,
                CompanyName = seller.CompanyName,
                Address = seller.Address,
                MobileNo = seller.MobileNo,
                Email = seller.Email,
                Rating = (double)seller.Rating,
                CreatedBy = seller.CreatedBy,
                CreatedDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(seller.CreatedDate.ToUniversalTime()),
                ModifiedDate = seller.ModifiedDate.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(seller.ModifiedDate.Value.ToUniversalTime()) : null,
                ModifiedBy = seller.ModifiedBy ?? 0,
                IsDeleted = seller.IsDeleted
            };
        }
    }

}
