using EfCoreTutorial.Entity.ECommerceModels;
using Grpc.Core;

namespace ProductServiceGrpc.Services
{
    public class SellerService : SellerService.SellerServiceBase
    {
        private readonly YourDbContext _db;

        public SellerService(YourDbContext db)
        {
            _db = db;
        }

        public override async Task<SellerResponse> CreateSeller(SellerCreateRequest request, ServerCallContext context)
        {
            var seller = new Seller
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                MobileNo = request.MobileNo,
                Email = request.Email,
                Rating = (decimal)request.Rating,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _db.Sellers.Add(seller);
            await _db.SaveChangesAsync();

            return MapToResponse(seller);
        }

        public override async Task<SellerResponse> GetSellerById(SellerRequest request, ServerCallContext context)
        {
            var seller = await _db.Sellers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted);

            return seller == null ? null : MapToResponse(seller);
        }

        public override async Task<SellerListResponse> GetAllSellers(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var sellers = await _db.Sellers
                .Where(s => !s.IsDeleted)
                .Include(s => s.Products)
                .ToListAsync();

            var response = new SellerListResponse();
            response.Sellers.AddRange(sellers.Select(MapToResponse));
            return response;
        }

        public override async Task<SellerResponse> UpdateSeller(SellerUpdateRequest request, ServerCallContext context)
        {
            var seller = await _db.Sellers.FindAsync(request.Id);
            if (seller == null || seller.IsDeleted) return null;

            seller.CompanyName = request.CompanyName;
            seller.Address = request.Address;
            seller.MobileNo = request.MobileNo;
            seller.Email = request.Email;
            seller.Rating = (decimal)request.Rating;
            seller.ModifiedBy = request.ModifiedBy;
            seller.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return MapToResponse(seller);
        }

        public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteSeller(SellerDeleteRequest request, ServerCallContext context)
        {
            var seller = await _db.Sellers.FindAsync(request.Id);
            if (seller != null && !seller.IsDeleted)
            {
                seller.IsDeleted = true;
                seller.ModifiedBy = request.ModifiedBy;
                seller.ModifiedDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return new Google.Protobuf.WellKnownTypes.Empty();
        }

        private SellerResponse MapToResponse(Seller seller)
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
