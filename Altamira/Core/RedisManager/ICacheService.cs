using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.RedisManager
{
    public interface ICacheService
    {
        T Get<T>(string cacheKey);
        void Set(string cacheKey, object model, TimeSpan time);
    }
}
