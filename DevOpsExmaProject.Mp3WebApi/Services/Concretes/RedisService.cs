using DevOpsExmaProject.Mp3WebApi.Services.Abstracts;
using StackExchange.Redis;

namespace DevOpsExmaProject.Mp3WebApi.Services.Concretes
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _redisDatabase;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        private string GetRedisKey(string userId) => $"user:{userId}:favorites";

        public void ChangeFavorite(string userId, int mp3Id)
        {
            string redisKey = GetRedisKey(userId);

            if (_redisDatabase.SetContains(redisKey, mp3Id))
            {
                _redisDatabase.SetRemove(redisKey, mp3Id);
            }
            else
            {
                _redisDatabase.SetAdd(redisKey, mp3Id);
            }
        }

        public bool CheckFavorite(string userId, int mp3Id)
        {
            string redisKey = GetRedisKey(userId);

            return _redisDatabase.SetContains(redisKey, mp3Id);
        }
    }
}
