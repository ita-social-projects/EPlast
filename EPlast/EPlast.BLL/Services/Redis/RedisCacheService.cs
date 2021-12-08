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
                if (!jsonData.HasValue)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Can not get records from redis, because {ex}");
                return default(T);
            }
        }

        public async Task RemoveRecordAsync(string recordId)
        {
            try
            {
                await _db.KeyDeleteAsync(recordId);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Can not delete keys from redis, because {ex}");
            }
        }

        public async Task RemoveRecordsByPatternAsync(string pattern)
        {
            try
            {
                var server = _connectionMultiplexer.GetServer(_configuration.GetConnectionString("Redis"));
                var keys = _server.Keys(pattern: pattern + "*", pageSize: 1000);
                foreach (var key in keys)
                {
                    await _db.KeyDeleteAsync(key);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Can not get server for redis instance, because {ex}");
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