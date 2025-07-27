using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using ProductServiceGrpc;

namespace API_Gateway.Services
{
    public interface ISellerService
    {
        Task<SellerResponse> CreateSellerAsync(SellerCreateRequest request);
        Task<SellerResponse> GetSellerByIdAsync(int sellerId);
        Task<List<SellerResponse>> GetAllSellersAsync();
        Task<SellerResponse> UpdateSellerAsync(SellerUpdateRequest request);
        Task DeleteSellerAsync(int sellerId, int modifiedBy);
    }

    public class SellerService : ISellerService
    {
        private readonly Seller.SellerClient _client;

        public SellerService()
        {
            string grpcServerAddress = "";
            var channel = GrpcChannel.ForAddress(grpcServerAddress);
            _client = new Seller.SellerClient(channel);
        }

        public async Task<SellerResponse> CreateSellerAsync(SellerCreateRequest request)
        {
            return await _client.CreateSellerAsync(request);
        }

        public async Task<SellerResponse> GetSellerByIdAsync(int sellerId)
        {
            var request = new SellerRequest { Id = sellerId };
            return await _client.GetSellerByIdAsync(request);
        }

        public async Task<List<SellerResponse>> GetAllSellersAsync()
        {
            var response = await _client.GetAllSellersAsync(new Empty());
            return new List<SellerResponse>(response.Sellers);
        }

        public async Task<SellerResponse> UpdateSellerAsync(SellerUpdateRequest request)
        {
            return await _client.UpdateSellerAsync(request);
        }

        public async Task DeleteSellerAsync(int sellerId, int modifiedBy)
        {
            var request = new SellerDeleteRequest
            {
                Id = sellerId,
                ModifiedBy = modifiedBy
            };
            await _client.DeleteSellerAsync(request);
        }
    }
}
