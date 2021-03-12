using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Hash
{
    public interface IHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashPassword);
    }
}
