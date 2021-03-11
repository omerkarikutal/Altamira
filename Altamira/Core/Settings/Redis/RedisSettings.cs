using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Settings.Redis
{
    public class RedisSettings
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
    }
}
