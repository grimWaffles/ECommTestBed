using EfCoreTutorial.Entity.ECommerceModels;
using Grpc.Core;

namespace ProductServiceGrpc.Services
{
    public class ProductCategoryService : ProductCategoryService.ProductCategoryServiceBase
    {
        private readonly YourDbContext _db;

        public ProductCategoryService(YourDbContext db)
        {
            _db = db;
        }

        public override async Task<ProductCategoryResponse> CreateCategory(ProductCategoryCreateRequest request, ServerCallContext context)
        {
            var category = new ProductCategory
            {
                CategoryName = request.CategoryName,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            _db.ProductCategories.Add(category);
            await _db.SaveChangesAsync();

            return MapToResponse(category);
        }

        public override async Task<ProductCategoryResponse> GetCategoryById(ProductCategoryRequest request, ServerCallContext context)
        {
            var category = await _db.ProductCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted);

            return category == null ? null : MapToResponse(category);
        }

        public override async Task<ProductCategoryListResponse> GetAllCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var categories = await _db.ProductCategories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            var response = new ProductCategoryListResponse();
            response.Categories.AddRange(categories.Select(MapToResponse));
            return response;
        }

        public override async Task<ProductCategoryResponse> UpdateCategory(ProductCategoryUpdateRequest request, ServerCallContext context)
        {
            var category = await _db.ProductCategories.FindAsync(request.Id);
            if (category == null || category.IsDeleted) return null;

            category.CategoryName = request.CategoryName;
            category.ModifiedBy = request.ModifiedBy;
            category.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return MapToResponse(category);
        }

        public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCategory(ProductCategoryDeleteRequest request, ServerCallContext context)
        {
            var category = await _db.ProductCategories.FindAsync(request.Id);
            if (category != null && !category.IsDeleted)
            {
                category.IsDeleted = true;
                category.ModifiedBy = request.ModifiedBy;
                category.ModifiedDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return new Google.Protobuf.WellKnownTypes.Empty();
        }

        private ProductCategoryResponse MapToResponse(ProductCategory category)
        {
            return new ProductCategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CreatedBy = category.CreatedBy,
                CreatedDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(category.CreatedDate.ToUniversalTime()),
                ModifiedDate = category.ModifiedDate.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(category.ModifiedDate.Value.ToUniversalTime()) : null,
                ModifiedBy = category.ModifiedBy ?? 0,
                IsDeleted = category.IsDeleted
            };
        }
    }

}
