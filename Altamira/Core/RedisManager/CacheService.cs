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
        public T Get<T>(string cacheKey)
        {
            var result = distributedCache.GetString(cacheKey);
            if (string.IsNullOrEmpty(result))
                return default;

            return JsonConvert.DeserializeObject<T>(result);
        }

        public void Set(string cacheKey, object model, TimeSpan time)
        {
            if (model == null)
                return;

            var serialize = JsonConvert.SerializeObject(model);

            distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serialize), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = time
            });
        }
    }
}
