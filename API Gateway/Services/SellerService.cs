using Grpc.Net.Client;
using ApiGateway.Protos;
using Google.Protobuf.WellKnownTypes;

public interface ISellerGrpcClient
{
    Task<SellerResponse> CreateSellerAsync(int userId, SellerDto dto);
    Task<SellerDto> GetSellerByIdAsync(int id);
    Task<List<SellerDto>> GetAllSellersAsync();
    Task<SellerResponse> UpdateSellerAsync(int userId, SellerDto dto);
    Task<SellerResponse> DeleteSellerAsync(int id, int userId);
}

public class SellerGrpcClient : ISellerGrpcClient
{
    private readonly Seller.SellerClient _client;

    public SellerGrpcClient(Seller.SellerClient client)
    {
        _client = client;
    }

    public async Task<SellerResponse> CreateSellerAsync(int userId, SellerDto dto)
    {
        var request = new SellerRequest { UserId = userId, Dto = dto };
        return await _client.CreateSellerAsync(request);
    }

    public async Task<SellerDto> GetSellerByIdAsync(int id)
    {
        return await _client.GetSellerByIdAsync(new SellerSingleRequest { Id = id });
    }

    public async Task<List<SellerDto>> GetAllSellersAsync()
    {
        var response = await _client.GetAllSellersAsync(new Empty());
        return response.Sellers.ToList();
    }

    public async Task<SellerResponse> UpdateSellerAsync(int userId, SellerDto dto)
    {
        var request = new SellerRequest { UserId = userId, Dto = dto };
        return await _client.UpdateSellerAsync(request);
    }

    public async Task<SellerResponse> DeleteSellerAsync(int id, int userId)
    {
        return await _client.DeleteSellerAsync(new SellerDeleteRequest { Id = id, UserId = userId });
    }
}
