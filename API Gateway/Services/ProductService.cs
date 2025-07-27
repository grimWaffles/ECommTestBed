using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using ProductServiceGrpc;

namespace API_Gateway.Services
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProductAsync(ProductCreateRequest request);
        Task<ProductResponse> GetProductByIdAsync(int productId);
        Task<List<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request);
        Task DeleteProductAsync(int productId, int modifiedBy);
    }
    public class ProductService : IProductService
    {
        private readonly Product.ProductClient _client;

        public ProductService()
        {
            string grpcServerAddress = "";
            var channel = GrpcChannel.ForAddress(grpcServerAddress);
            _client = new Product.ProductClient(channel);
        }

        public async Task<ProductResponse> CreateProductAsync(ProductCreateRequest request)
        {
            return await _client.CreateProductAsync(request);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int productId)
        {
            var request = new ProductRequest { Id = productId };
            return await _client.GetProductByIdAsync(request);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var response = await _client.GetAllProductsAsync(new Empty());
            return new List<ProductResponse>(response.Products);
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request)
        {
            return await _client.UpdateProductAsync(request);
        }

        public async Task DeleteProductAsync(int productId, int modifiedBy)
        {
            var request = new ProductDeleteRequest
            {
                Id = productId,
                ModifiedBy = modifiedBy
            };
            await _client.DeleteProductAsync(request);
        }
    }
}
