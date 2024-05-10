using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Domain.Interfaces.Repository;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knights.Challenge.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public BaseRepository(IMongoDatabase mongoDatabase)
        {
            var collectionName = typeof(TEntity).Name.Replace("Entity", "");
            _collection = mongoDatabase.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> CreateCollection(TEntity collection)
        {
            await _collection.InsertOneAsync(collection);
            return collection;
        }

        public async Task DeleteCollection(string id)
        {
            await _collection.DeleteOneAsync(k => k.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllCollections()
        {
            return await _collection.Find(k => true).ToListAsync();
        }

        public async Task<TEntity> GetCollectionById(string id)
        {
            return await _collection.Find(k => k.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateCollection(string id, TEntity collection)
        {
            await _collection.ReplaceOneAsync(k => k.Id == id, collection);
        }
    }
}
