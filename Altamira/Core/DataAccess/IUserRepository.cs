using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IUserRepository : IRepository<User>
    {
        Task AddRangeAsync(List<User> users);
    }
}
