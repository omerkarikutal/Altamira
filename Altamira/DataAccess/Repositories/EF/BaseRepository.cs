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
            try
            {
                await Collection.InsertOneAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                throw new MongoException("Database Error");//todo
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                await Collection.DeleteOneAsync(s => s.Id == entity.Id);
            }
            catch (Exception)
            {
                throw new MongoException("Database Error");//todo
            }
        }

        public async Task<List<T>> Get()
        {
            try
            {
                return await Collection.Find(T => true).ToListAsync();
            }
            catch (Exception)
            {
                throw new MongoException("Database Error");//todo
            }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await Collection.Find(predicate).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new MongoException("Database Error");//todo
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
                return entity;
            }
            catch (Exception)
            {
                throw new MongoException("Database Error");//todo
            }
        }
    }
}
