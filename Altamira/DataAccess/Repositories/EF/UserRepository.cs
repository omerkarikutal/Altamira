using Core.DataAccess;
using Core.Entities;
using Core.Settings.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.EF
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {

        }
    }
}
