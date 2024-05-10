using System.Collections.Generic;
using System.Threading.Tasks;
using Knights.Challenge.Domain.Entities;

namespace Knights.Challenge.Domain.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllCollections();
        Task<TEntity> GetCollectionById(string id);
        Task<TEntity> CreateCollection(TEntity Collection);
        Task UpdateCollection(string id, TEntity Collection);
        Task DeleteCollection(string id);

    }
}
