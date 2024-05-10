using Knights.Challenge.Domain.DTOs.Request;
using Knights.Challenge.Domain.DTOs.Response;
using Knights.Challenge.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knights.Challenge.Domain.Interfaces.Service
{
    public interface IKnightService
    {
        Task<IEnumerable<KnightResponseDTO>> GetAllKnights();
        Task<IEnumerable<KnightResponseDTO>> GetHeroKnights();
        Task<KnightResponseDTO> GetKnightById(string id);
        Task<KnightResponseDTO> CreateKnight(KnightRequestDTO knight);
        Task UpdateKnight(string id, KnightRequestDTO knightRequestDTO);
        Task<KnightResponseDTO> UpdateKnightNickname(string id, NewNicknameRequestDTO newNicknameRequestDTO);
        Task DeleteKnight(string id);
        Task<bool> UpdateKnightDeleted(string id);
    }
}
