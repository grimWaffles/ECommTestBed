using API_Gateway.Models;
using ApiGateway.Protos;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace API_Gateway.Services
{
    public interface ICartService
    {
        Task<bool> SaveCartAsync(Cart cart);
        Task<Cart?> GetCartAsync(int userId);
        Task<bool> DeleteCartAsync(int userId);
        Task<List<Cart>> GetAllCartsAsync();
        Task<bool> AddItemToCartAsync(int userId, OrderItem item);
        Task<bool> RemoveItemFromCartAsync(int userId, int orderItemId);
    }

    public class CartService : ICartService
    {
        private readonly IConfiguration _config;
        private readonly ConnectionMultiplexer _redisConnector;
        private readonly IDatabase _redis;
        private const string CartIndexKey = "cart:index";

        public CartService(IConfiguration config)
        {
            _config = config;
            _redisConnector = ConnectionMultiplexer.Connect(_config["Redis:url"]);
            _redis = _redisConnector.GetDatabase();
        }

        private string GetCartKey(int userId) => $"cart:{userId}";

        // Create or Update Cart
        public async Task<bool> SaveCartAsync(Cart cart)
        {
            try
            {
                if (cart == null || cart.UserId <= 0)
                    return false;

                cart.RecalculateTotal();
                string key = GetCartKey(cart.UserId);

                string json = JsonSerializer.Serialize(cart);
                await _redis.StringSetAsync(key, json);

                // Maintain index of cart keys
                await _redis.SetAddAsync(CartIndexKey, key);
                return true;
            }
            catch (Exception ex)
            {
                // log ex (replace with your logging)
                Console.WriteLine($"Error saving cart: {ex.Message}");
                return false;
            }
        }

        // Read Cart
        public async Task<Cart?> GetCartAsync(int userId)
        {
            try
            {
                string key = GetCartKey(userId);
                string? json = await _redis.StringGetAsync(key);
                if (string.IsNullOrEmpty(json))
                    return null;

                return JsonSerializer.Deserialize<Cart>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cart: {ex.Message}");
                return null;
            }
        }

        // Delete Cart
        public async Task<bool> DeleteCartAsync(int userId)
        {
            try
            {
                string key = GetCartKey(userId);
                bool deleted = await _redis.KeyDeleteAsync(key);

                // Remove from index if exists
                await _redis.SetRemoveAsync(CartIndexKey, key);
                return deleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting cart: {ex.Message}");
                return false;
            }
        }

        // Get All Carts (optional - admin use)
        public async Task<List<Cart>> GetAllCartsAsync()
        {
            var carts = new List<Cart>();
            try
            {
                var keys = await _redis.SetMembersAsync(CartIndexKey);
                foreach (string key in keys)
                {
                    string? json = await _redis.StringGetAsync(key);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var cart = JsonSerializer.Deserialize<Cart>(json);
                        if (cart != null)
                            carts.Add(cart);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all carts: {ex.Message}");
            }
            return carts;
        }

        // Add Item to Cart
        public async Task<bool> AddItemToCartAsync(int userId, OrderItem item)
        {
            try
            {
                var cart = await GetCartAsync(userId) ?? new Cart { UserId = userId };
                cart.Items.Add(item);
                return await SaveCartAsync(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to cart: {ex.Message}");
                return false;
            }
        }

        // Remove Item from Cart
        public async Task<bool> RemoveItemFromCartAsync(int userId, int orderItemId)
        {
            try
            {
                var cart = await GetCartAsync(userId);
                if (cart == null) return false;

                cart.Items.RemoveAll(i => i.Id == orderItemId);
                return await SaveCartAsync(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
                return false;
            }
        }
    }
}