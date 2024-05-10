using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Domain.Interfaces.Repository;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knights.Challenge.Data.Repositories
{
    public class KnightRepository : BaseRepository<KnightEntity>, IKnightRepository
    {
        public KnightRepository(IMongoDatabase collection) : base(collection) { }

        public async Task<IEnumerable<KnightEntity>> GetHeroKnights()
        {
            return await _collection.Find(knight => knight.Deleted).ToListAsync();
        }

        public async Task<IEnumerable<KnightEntity>> GetKnights()
        {
            return await _collection.Find(knight => !knight.Deleted).ToListAsync();
        }
    }
}
