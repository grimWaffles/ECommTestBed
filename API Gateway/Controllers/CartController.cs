using API_Gateway.Models;
using API_Gateway.Services;
using ApiGateway.Protos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Get a user's cart
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null)
                return NotFound($"No cart found for user {userId}");
            return Ok(cart);
        }

        // Save (create/update) a cart
        [HttpPost]
        public async Task<IActionResult> SaveCart([FromBody] Cart cart)
        {
            if (cart == null || cart.UserId <= 0)
                return BadRequest("Invalid cart data.");

            var success = await _cartService.SaveCartAsync(cart);
            if (!success)
                return StatusCode(500, "Failed to save cart.");
            return Ok("Cart saved successfully.");
        }

        // Delete a cart
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteCart(int userId)
        {
            var deleted = await _cartService.DeleteCartAsync(userId);
            if (!deleted)
                return NotFound($"No cart found for user {userId}");
            return Ok($"Cart for user {userId} deleted.");
        }

        // Add item to a cart
        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddItem(int userId, [FromBody] OrderItem item)
        {
            if (item == null)
                return BadRequest("Invalid item data.");

            var success = await _cartService.AddItemToCartAsync(userId, item);
            if (!success)
                return StatusCode(500, "Failed to add item to cart.");
            return Ok("Item added successfully.");
        }

        // Remove item from a cart
        [HttpDelete("{userId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int userId, int itemId)
        {
            var success = await _cartService.RemoveItemFromCartAsync(userId, itemId);
            if (!success)
                return NotFound($"Item {itemId} not found in user {userId}'s cart.");
            return Ok($"Item {itemId} removed from cart.");
        }

        // (Optional) Get all carts (admin endpoint)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCarts()
        {
            var carts = await _cartService.GetAllCartsAsync();
            return Ok(carts);
        }
    }

}