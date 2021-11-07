using AutoMapper.Internal;
using EPlast.BLL.Interfaces.Cache;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly IServer _server;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, IConfiguration Configuration)
        {
            _db = connectionMultiplexer.GetDatabase();
            _server = connectionMultiplexer.GetServer(Configuration.GetConnectionString("Redis"));
        }

        public async Task<bool> CheckIfKeyExistsAsync(string recordId)
        {
            return await _db.KeyExistsAsync(recordId);
        }

        public async Task<T> GetRecordByKeyAsync<T>(string recordId)
        {
            var jsonData = await _db.StringGetAsync(recordId);
            if (!jsonData.HasValue)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public async Task RemoveRecordAsync(string recordId)
        {
           await _db.KeyDeleteAsync(recordId);
        }

        public async Task RemoveRecordsByPatternAsync(string pattern)
        {
            var keys = _server.Keys(pattern: pattern + "*", pageSize: 1000);
            foreach(var key in keys)
            {
                await _db.KeyDeleteAsync(key);
            }
        }

        public async Task SetCacheRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            absoluteExpireTime = absoluteExpireTime ?? TimeSpan.FromHours(1);

            await _db.StringSetAsync(recordId, jsonData, absoluteExpireTime);
        }
    }
}