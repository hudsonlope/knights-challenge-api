using Knights.Challenge.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knights.Challenge.Domain.Interfaces.Repository
{
    public interface IKnightRepository : IBaseRepository<KnightEntity>
    {
        Task<IEnumerable<KnightEntity>> GetHeroKnights();
        Task<IEnumerable<KnightEntity>> GetKnights();
    }
}
