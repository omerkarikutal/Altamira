using Core.DataAccess;
using Core.Entities;
using Core.Settings.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.EF
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {

        }

        public async Task AddRangeAsync(List<User> users)
        {
            await Collection.InsertManyAsync(users);
        }
    }
}
