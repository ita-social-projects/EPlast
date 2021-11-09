using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Cache
{
    public interface ICacheService
    {
        public Task SetCacheRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null);
        public Task<T> GetRecordByKeyAsync<T>(string recordId);
        public Task RemoveRecordsByPatternAsync(string pattern);
        public Task<bool> CheckIfKeyExistsAsync(string recordId);
        public Task RemoveRecordAsync(string recordId);
    }
}
