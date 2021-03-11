using System;
using System.Collections.Generic;
using System.Text;

namespace Core.RedisManager
{
    public class RedisException : Exception
    {
        public RedisException() : base() { }

        public RedisException(string message) : base(message) { }
    }
}
