using System.Threading.Tasks;
using API_Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisTestController(IRedisService redisService) : ControllerBase
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
            _redis.SetValueByKey("Company", "Datasoft");
            return Ok("Functions complete");
        }

        [HttpGet]
        [Route("get-key")]
        public async Task<IActionResult> GetKey()
        {
            string key = "Company";
            if (_redis.DoesKeyExist(key))
            {
                return Ok(await _redis.GetValueByKey(key));
            }
            return Ok("No key exists.");
        }
    }
}