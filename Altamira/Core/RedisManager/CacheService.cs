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
            try
            {
                var result = distributedCache.GetString(cacheKey);
                if (string.IsNullOrEmpty(result))
                    return default;

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception)
            {
                throw new RedisException("Redis Error");//todo
            }
        }

        public void Set(string cacheKey, object model, TimeSpan time)
        {
            try
            {
                if (model == null)
                    return;

                var serialize = JsonConvert.SerializeObject(model);

                distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serialize), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = time
                });
            }
            catch (Exception)
            {
                throw new RedisException("Redis Error");//todo
            }
        }
    }
}
