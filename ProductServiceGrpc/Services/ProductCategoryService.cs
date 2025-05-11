
using EfCoreTutorial.Entity.ECommerceModels;
using Grpc.Core;
using ProductServiceGrpc.Database;
using ProductServiceGrpc.Repository;

namespace ProductServiceGrpc.Services
{
    public class ProductCategoryService : ProductCategory.ProductCategoryBase
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoryService(IProductCategoryRepository db)
        {
            _repo = db;
        }

        public override async Task<ProductCategoryResponse> CreateCategory(ProductCategoryCreateRequest request, ServerCallContext context)
        {
            ProductCategoryModel category = new ProductCategoryModel()
            {
                CategoryName = request.CategoryName,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                await _repo.CreateCategoryAsync(category);
            }
            catch(Exception e)
            {
                return null;
            }

            return MapToResponse(category);
        }

        public override async Task<ProductCategoryResponse> GetCategoryById(ProductCategoryRequest request, ServerCallContext context)
        {
            ProductCategoryModel category = await _repo.GetCategoryByIdAsync(request.Id);

            return category == null ? null : MapToResponse(category);
        }

        public override async Task<ProductCategoryListResponse> GetAllCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var categories = await _repo.GetAllCategoriesAsync();

            var response = new ProductCategoryListResponse();
            response.Categories.AddRange(categories.Select(MapToResponse));
            return response;
        }

        public override async Task<ProductCategoryResponse> UpdateCategory(ProductCategoryUpdateRequest request, ServerCallContext context)
        {
            var category = await _repo.GetCategoryByIdAsync(request.Id);

            if (category == null || category.IsDeleted) return null;

            category.CategoryName = request.CategoryName;
            category.ModifiedBy = request.ModifiedBy;
            category.ModifiedDate = DateTime.UtcNow;

            try
            {
                await _repo.UpdateCategoryAsync(category);
            }
            catch(Exception e)
            {
                return null;
            }

            return MapToResponse(category);
        }

        public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCategory(ProductCategoryDeleteRequest request, ServerCallContext context)
        {
            var category = await _repo.GetCategoryByIdAsync(request.Id);

            try
            {
                if (category != null && !category.IsDeleted)
                {
                    await _repo.DeleteCategoryAsync(request.Id, request.ModifiedBy);
                }
            }
            catch (Exception ex) {
                return null;
            }

            return new Google.Protobuf.WellKnownTypes.Empty();
        }

        private ProductCategoryResponse MapToResponse(ProductCategoryModel category)
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
