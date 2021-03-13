using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.RedisManager
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache distributedCache;
        public CacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public bool Exists(string cacheKey)
        {
            if (distributedCache.Get(cacheKey) != null)
                return true;
            return false;
        }

        public T Get<T>(string cacheKey)
        {
            if (!Exists(cacheKey))
                return default;

            var result = distributedCache.GetString(cacheKey);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public void Remove(string cacheKey)
        {
            if (Exists(cacheKey))
                distributedCache.Remove(cacheKey);
        }

        public void Set(string cacheKey, object model, TimeSpan time)
        {
            if (model == null)
                return;

            var serialize = JsonConvert.SerializeObject(model);

            distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serialize),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = time
                });
        }
    }
}
