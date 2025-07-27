using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ProductServiceGrpc;

namespace API_Gateway.Services
{
    public interface IProductCategoryGrpcClient
    {
        Task<ProductCategoryCreateResponse> CreateCategoryAsync(int userId, ProductCategoryDto dto);
        Task<ProductCategoryDto> GetCategoryByIdAsync(int id);
        Task<List<ProductCategoryDto>> GetAllCategoriesAsync();
        Task<ProductCategoryCreateResponse> UpdateCategoryAsync(int userId, ProductCategoryDto dto);
        Task<ProductCategoryCreateResponse> DeleteCategoryAsync(int id, int userId);
    }

    public class ProductCategoryGrpcClient : IProductCategoryGrpcClient
    {
        private readonly ProductCategory.ProductCategoryClient _client;

        public ProductCategoryGrpcClient(ProductCategory.ProductCategoryClient client)
        {
            _client = client;
        }

        public async Task<ProductCategoryCreateResponse> CreateCategoryAsync(int userId, ProductCategoryDto dto)
        {
            var request = new ProductCategoryCreateRequest
            {
                UserId = userId,
                Dto = dto
            };
            return await _client.CreateCategoryAsync(request);
        }

        public async Task<ProductCategoryDto> GetCategoryByIdAsync(int id)
        {
            var request = new ProductCategorySingleRequest { Id = id };
            return await _client.GetCategoryByIdAsync(request);
        }

        public async Task<List<ProductCategoryDto>> GetAllCategoriesAsync()
        {
            var response = await _client.GetAllCategoriesAsync(new Empty());
            return response.Dtos.ToList();
        }

        public async Task<ProductCategoryCreateResponse> UpdateCategoryAsync(int userId, ProductCategoryDto dto)
        {
            var request = new ProductCategoryCreateRequest
            {
                UserId = userId,
                Dto = dto
            };
            return await _client.UpdateCategoryAsync(request);
        }

        public async Task<ProductCategoryCreateResponse> DeleteCategoryAsync(int id, int userId)
        {
            var request = new ProductCategoryDeleteRequest
            {
                Id = id,
                Userid = userId
            };
            return await _client.DeleteCategoryAsync(request);
        }
    }
}
