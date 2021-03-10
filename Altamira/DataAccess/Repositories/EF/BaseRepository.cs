using Core.DataAccess;
using Core.Entities;
using Core.Settings.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.EF
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> Collection;
        protected BaseRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var db = client.GetDatabase(databaseSettings.DatabaseName);
            this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }
        public async Task<T> AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            await Collection.DeleteOneAsync(s => s.Id == entity.Id);
        }

        public async Task<List<T>> Get()
        {
            return await Collection.Find(T => true).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            return entity;
        }
    }
}
