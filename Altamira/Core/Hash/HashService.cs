using System;
using System.Collections.Generic;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace Core.Hash
{
    public class HashService : IHashService
    {
        public string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BC.Verify(password, hashPassword);
        }
    }
}
