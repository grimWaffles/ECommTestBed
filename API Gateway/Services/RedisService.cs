using StackExchange.Redis;

namespace API_Gateway.Services
{
    public interface IRedisService
    {
        Task<string> GetValueByKey(string key);
        string SetValueByKey(string keyName, string keyValue);
        bool DoesKeyExist(string key);
        bool DeleteKey(string key);
    }
    public class RedisService : IRedisService
    {
        private readonly IConfiguration _config;
        private readonly ConnectionMultiplexer _redisConnector;
        private readonly IDatabase _redis;

        public RedisService(IConfiguration config)
        {
            _config = config;
            _redisConnector = ConnectionMultiplexer.Connect(_config["Redis:url"]);
            _redis = _redisConnector.GetDatabase();
        }

        public async Task<string> GetValueByKey(string key)
        {
            try
            {
                return await _redis.StringGetAsync(key);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SetValueByKey(string keyName, string keyValue)
        {
            try
            {
                bool keyAdded = _redis.StringSet(keyName, keyValue);
                return keyAdded ? "Success" : "Failed";
            }
            catch (Exception e)
            {
                return "Failed";
            }
        }

        public bool DoesKeyExist(string key)
        {
            return _redis.KeyExists(key);
        }

        public bool DeleteKey(string key)
        {
            if (_redis.KeyExists(key))
            {
                _redis.KeyDelete(key);
                return true;
            }

            return false;
        }
    }
}