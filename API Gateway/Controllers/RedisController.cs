using System.Text.Json;
using System.Threading.Tasks;
using API_Gateway.Models;
using API_Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisController(IRedisService redisService) : ControllerBase
    {
        private readonly IRedisService _redis = redisService;

        [HttpGet]
        [Route("test")]
        public IActionResult TestController()
        {
            return Ok("Controller Functional");
        }

        [HttpGet]
        [Route("add-key")]
        public async Task<IActionResult> AddKeyToRedis()
        {
            List<RedisTestModel> list = new List<RedisTestModel>();

            for (int i = 1; i < 100; i++)
            {
                RedisTestModel model = new RedisTestModel()
                {
                    Id = i,
                    Username = "Wazi",
                    IsRegistered = true,
                    RegistrationDate = DateTime.Now
                };

                Console.WriteLine($"Adding model with ID:{model.Id}");
                list.Add(model);
            }

            _redis.SetValueByKey("user_obj", JsonSerializer.Serialize(list));
            return Ok("Functions complete");
        }

        [HttpGet]
        [Route("get-key")]
        public async Task<IActionResult> GetKey()
        {
            string key = "user_obj";
            if (_redis.DoesKeyExist(key))
            {
                var redisList = await _redis.GetValueByKey(key);
                List<RedisTestModel> list = JsonSerializer.Deserialize<List<RedisTestModel>>(redisList);
                return Ok(list);
            }
            return Ok("No key exists.");
        }
    }
}