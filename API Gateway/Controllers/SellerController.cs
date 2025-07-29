using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiGateway.Protos;
using System.Security.Claims;

[ApiController]
[Route("api/sellers")]
[Authorize]
public class SellersController : ControllerBase
{
    private readonly ISellerGrpcClient _grpcClient;

    public SellersController(ISellerGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    // Helper to extract userId from JWT claims
    private int GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // or "userId"
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new UnauthorizedAccessException("Invalid or missing user ID claim");
        return userId;
    }

    // POST: api/sellers/create
    [HttpPost("create")]
    public async Task<IActionResult> CreateSeller([FromForm] SellerDto dto)
    {
        try
        {
            int userId = GetUserIdFromClaims();
            var response = await _grpcClient.CreateSellerAsync(userId, dto);
            return response.Status == 1 ? Ok(response.Dto) : BadRequest(response.ErrorMessage);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    // GET: api/sellers/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSellerById([FromRoute] int id)
    {
        var seller = await _grpcClient.GetSellerByIdAsync(id);
        return seller != null && seller.Id > 0 ? Ok(seller) : NotFound();
    }

    // GET: api/sellers/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSellers()
    {
        var sellers = await _grpcClient.GetAllSellersAsync();
        return Ok(sellers);
    }

    // PUT: api/sellers/update/{id}
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateSeller([FromRoute] int id, [FromForm] SellerDto dto)
    {
        try
        {
            int userId = GetUserIdFromClaims();
            dto.Id = id;
            var response = await _grpcClient.UpdateSellerAsync(userId, dto);
            return response.Status == 1 ? Ok(response.Dto) : BadRequest(response.ErrorMessage);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    // DELETE: api/sellers/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteSeller([FromRoute] int id)
    {
        try
        {
            int userId = GetUserIdFromClaims();
            var response = await _grpcClient.DeleteSellerAsync(id, userId);
            return response.Status == 1 ? Ok() : BadRequest(response.ErrorMessage);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}
