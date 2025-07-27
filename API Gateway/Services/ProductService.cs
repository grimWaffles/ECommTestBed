using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using ProductServiceGrpc;

namespace API_Gateway.Services
{
    public interface IProductGrpcClient
    {
        Task<ProductResponse> CreateProductAsync(int userId, ProductDto dto);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductResponse> UpdateProductAsync(int userId, ProductDto dto);
        Task<ProductResponse> DeleteProductAsync(int id, int modifiedBy);
    }
    public class ProductGrpcClient : IProductGrpcClient
    {
        private readonly ProductService.ProductServiceClient _client;

        public ProductGrpcClient(ProductService.ProductServiceClient client)
        {
            _client = client;
        }

        public async Task<ProductResponse> CreateProductAsync(int userId, ProductDto dto)
        {
            var request = new ProductRequest { UserId = userId, Dto = dto };
            return await _client.CreateProductAsync(request);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var request = new ProductIdRequest { Id = id };
            return await _client.GetProductByIdAsync(request);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var response = await _client.GetAllProductsAsync(new Empty());
            return response.Dtos.ToList();
        }

        public async Task<ProductResponse> UpdateProductAsync(int userId, ProductDto dto)
        {
            var request = new ProductRequest { UserId = userId, Dto = dto };
            return await _client.UpdateProductAsync(request);
        }

        public async Task<ProductResponse> DeleteProductAsync(int id, int modifiedBy)
        {
            var request = new ProductDeleteRequest { Id = id, ModifiedBy = modifiedBy };
            return await _client.DeleteProductAsync(request);
        }
    }
}
