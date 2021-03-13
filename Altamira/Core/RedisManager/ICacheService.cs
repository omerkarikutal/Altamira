using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.RedisManager
{
    public interface ICacheService
    {
        Task<T> Get<T>(string cacheKey);
        Task Set(string cacheKey, object model, TimeSpan time);
        Task Remove(string cacheKey);
        Task<bool> Exists(string cacheKey);
    }
}
