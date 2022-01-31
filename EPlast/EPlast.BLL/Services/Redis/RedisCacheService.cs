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
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration)
        {
            _db = connectionMultiplexer.GetDatabase();
            _configuration = configuration;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<bool> CheckIfKeyExistsAsync(string recordId)
        {
            try
            {
                return await _db.KeyExistsAsync(recordId);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Can not get keys from redis, because {ex}");
                return false;
            }

        }

        public async Task<T> GetRecordByKeyAsync<T>(string recordId)
        {
            try
            {
                var jsonData = await _db.StringGetAsync(recordId);
                return jsonData.HasValue ? JsonConvert.DeserializeObject<T>(jsonData) : default;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Can not get records from redis, because {ex}");
                return default;
            }
        }

        public async Task RemoveRecordAsync(string recordId)
        {
            await _db.KeyDeleteAsync(recordId);
        }

        public async Task RemoveRecordsByPatternAsync(string pattern)
        {
            try
            {
                var server = _connectionMultiplexer.GetServer(_configuration.GetConnectionString("Redis"));
                var keys = server.Keys(pattern: pattern + "*", pageSize: 1000);
                foreach (var key in keys)
                {
                    await _db.KeyDeleteAsync(key);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not get server, because {ex}");
            }
        }

        public async Task SetCacheRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            absoluteExpireTime = absoluteExpireTime ?? TimeSpan.FromHours(1);
            try
            {
                await _db.StringSetAsync(recordId, jsonData, absoluteExpireTime);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Can not set data, because {ex}");
            }
        }
    }
}