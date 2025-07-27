using API_Gateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductServiceGrpc;
using System.Security.Claims;

[ApiController]
[Route("api/product-categories")]
[Authorize]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryGrpcClient _grpcClient;

    public ProductCategoryController(IProductCategoryGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier); // or "userId"
        if (claim == null || !int.TryParse(claim.Value, out int userId))
            throw new UnauthorizedAccessException("Invalid or missing user ID claim.");
        return userId;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromForm] ProductCategoryDto dto)
    {
        int userId = GetUserId();
        var response = await _grpcClient.CreateCategoryAsync(userId, dto);
        return response.Status == 1 ? Ok(response.Dto) : BadRequest(response.ErrorMessage);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var dto = await _grpcClient.GetCategoryByIdAsync(id);
        return dto != null && dto.Id > 0 ? Ok(dto) : NotFound();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _grpcClient.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromForm] ProductCategoryDto dto)
    {
        int userId = GetUserId();
        dto.Id = id;
        var response = await _grpcClient.UpdateCategoryAsync(userId, dto);
        return response.Status == 1 ? Ok(response.Dto) : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        int userId = GetUserId();
        var response = await _grpcClient.DeleteCategoryAsync(id, userId);
        return response.Status == 1 ? Ok() : BadRequest(response.ErrorMessage);
    }
}
