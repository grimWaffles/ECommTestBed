using EfCoreTutorial.Entity.ECommerceModels;
using Grpc.Core;
using ProductServiceGrpc;
using ProductServiceGrpc.Repository;

namespace ProductServiceGrpc.Services
{
    public class ProductService : Product.ProductBase
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repository)
        {
            _repo = repository;
        }

        public override async Task<ProductResponse> CreateProduct(ProductCreateRequest request, ServerCallContext context)
        {
            ProductModel product = new ProductModel
            {
                Name = request.Name,
                DefaultQuantity = request.DefaultQuantity,
                Rating = (decimal)request.Rating,
                Price = (decimal)request.Price,
                Description = request.Description,
                SellerId = request.SellerId,
                ProductCategoryId = request.ProductCategoryId,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                await _repo.CreateProductAsync(product);

                return MapToResponse(product);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public override async Task<ProductResponse?> GetProductById(ProductRequest request, ServerCallContext context)
        {
            try
            {
                ProductModel product = await _repo.GetProductByIdAsync(request.Id);

                if (product == null)
                {
                    return null;
                }

                return MapToResponse(product);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public override async Task<ProductListResponse> GetAllProducts(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var products = await _repo.GetAllProductsAsync();

            var response = new ProductListResponse();
            response.Products.AddRange(products.Select(MapToResponse));
            return response;
        }

        public override async Task<ProductResponse> UpdateProduct(ProductUpdateRequest request, ServerCallContext context)
        {
            try
            {
                var product = await _repo.GetProductByIdAsync(request.Id);

                if (product == null || product.IsDeleted) return null;

                await _repo.UpdateProductAsync(product);

                return MapToResponse(product);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteProduct(ProductDeleteRequest request, ServerCallContext context)
        {
            var product = await _repo.GetProductByIdAsync(request.Id);

            if (product != null && !product.IsDeleted)
            {
                await _repo.DeleteProductAsync(request.Id, request.ModifiedBy);
            }

            return new Google.Protobuf.WellKnownTypes.Empty();
        }

        private ProductResponse MapToResponse(ProductModel product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                DefaultQuantity = product.DefaultQuantity ?? 0,
                Rating = (double)product.Rating,
                Price = (double)product.Price,
                Description = product.Description ?? "",
                SellerId = product.SellerId,
                ProductCategoryId = product.ProductCategoryId,
                CreatedBy = product.CreatedBy,
                CreatedDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(product.CreatedDate.ToUniversalTime()),
                ModifiedDate = product.ModifiedDate.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(product.ModifiedDate.Value.ToUniversalTime()) : null,
                ModifiedBy = product.ModifiedBy ?? 0,
                IsDeleted = product.IsDeleted,
                SellerCompanyName = product.Seller?.CompanyName ?? "",
                ProductCategoryName = product.ProductCategory?.CategoryName ?? ""
            };
        }
    }

}
