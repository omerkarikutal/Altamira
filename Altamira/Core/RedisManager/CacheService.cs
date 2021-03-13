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

        public async Task<bool> Exists(string cacheKey)
        {
            if (await distributedCache.GetAsync(cacheKey) != null)
                return true;
            return false;
        }

        public async Task<T> Get<T>(string cacheKey)
        {
            bool exists = await Exists(cacheKey);
            if (!exists)
                return default;

            var result = await distributedCache.GetStringAsync(cacheKey);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task Remove(string cacheKey)
        {
            bool exists = await Exists(cacheKey);

            if (exists)
                await distributedCache.RemoveAsync(cacheKey);
        }

        public async Task Set(string cacheKey, object model, TimeSpan time)
        {
            if (model == null)
                return;

            var serialize = JsonConvert.SerializeObject(model);

            await distributedCache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(serialize),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = time
                });
        }
    }
}
