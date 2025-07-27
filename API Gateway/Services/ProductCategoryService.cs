using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ProductServiceGrpc;

namespace API_Gateway.Services
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryResponse> CreateCategoryAsync(ProductCategoryCreateRequest request);
        Task<ProductCategoryResponse> GetCategoryByIdAsync(int categoryId);
        Task<List<ProductCategoryResponse>> GetAllCategoriesAsync();
        Task<ProductCategoryResponse> UpdateCategoryAsync(ProductCategoryUpdateRequest request);
        Task DeleteCategoryAsync(int categoryId, int modifiedBy);
    }
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ProductCategory.ProductCategoryClient _client;

        public ProductCategoryService()
        {
            string grpcServerAddress = "";
            var channel = GrpcChannel.ForAddress(grpcServerAddress);
            _client = new ProductCategory.ProductCategoryClient(channel);
        }

        public async Task<ProductCategoryResponse> CreateCategoryAsync(ProductCategoryCreateRequest request)
        {
            return await _client.CreateCategoryAsync(request);
        }

        public async Task<ProductCategoryResponse> GetCategoryByIdAsync(int categoryId)
        {
            var request = new ProductCategoryRequest { Id = categoryId };
            return await _client.GetCategoryByIdAsync(request);
        }

        public async Task<List<ProductCategoryResponse>> GetAllCategoriesAsync()
        {
            var response = await _client.GetAllCategoriesAsync(new Empty());
            return new List<ProductCategoryResponse>(response.Categories);
        }

        public async Task<ProductCategoryResponse> UpdateCategoryAsync(ProductCategoryUpdateRequest request)
        {
            return await _client.UpdateCategoryAsync(request);
        }

        public async Task DeleteCategoryAsync(int categoryId, int modifiedBy)
        {
            var request = new ProductCategoryDeleteRequest
            {
                Id = categoryId,
                ModifiedBy = modifiedBy
            };
            await _client.DeleteCategoryAsync(request);
        }
    }
}
