using AutoMapper;
using Knights.Challenge.Domain.Configuration;
using Knights.Challenge.Domain.DTOs.Request;
using Knights.Challenge.Domain.DTOs.Response;
using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Domain.Interfaces.Cache;
using Knights.Challenge.Domain.Interfaces.Repository;
using Knights.Challenge.Domain.Interfaces.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knights.Challenge.Service.Services
{
    public class KnightService : IKnightService
    {
        private readonly IKnightRepository _knightRepository;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;

        public KnightService(IKnightRepository knightRepository, IRedisService redisService, IMapper mapper)
        {
            _knightRepository = knightRepository;
            _redisService = redisService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<KnightResponseDTO>> GetAllKnights()
        {
            string hash = Constants.AllKnights;
            var knightsCache = await _redisService.GetValue(hash);

            IEnumerable<KnightEntity> knights;
            if (knightsCache != null && !string.IsNullOrWhiteSpace(knightsCache.Replace("[]", "")))
            {
                knights = JsonConvert.DeserializeObject<IEnumerable<KnightEntity>>(knightsCache);
            }
            else
            {
                knights = await _knightRepository.GetKnights();

                if (knights.Any())
                    await _redisService.SetValue(hash, JsonConvert.SerializeObject(knights));
            }

            return _mapper.Map<IEnumerable<KnightResponseDTO>>(knights);
        }

        public async Task<IEnumerable<KnightResponseDTO>> GetHeroKnights()
        {
            string hash = Constants.AllHeros;
            var heroKnightsCache = await _redisService.GetValue(hash);

            IEnumerable<KnightEntity> heroKnights;
            if (heroKnightsCache != null && !string.IsNullOrWhiteSpace(heroKnightsCache.Replace("[]", "")))
            {
                heroKnights = JsonConvert.DeserializeObject<IEnumerable<KnightEntity>>(heroKnightsCache);
            }
            else
            {
                heroKnights = await _knightRepository.GetHeroKnights();

                if (heroKnights.Any())
                    await _redisService.SetValue(hash, JsonConvert.SerializeObject(heroKnights));
            }

            return _mapper.Map<IEnumerable<KnightResponseDTO>>(heroKnights);
        }

        public async Task<KnightResponseDTO> GetKnightById(string id)
        {
            var knightCache = await _redisService.GetValue(id);

            KnightEntity? knight;
            if (knightCache != null && !string.IsNullOrWhiteSpace(knightCache.Replace("[]", "")))
            {
                knight = JsonConvert.DeserializeObject<KnightEntity>(knightCache);
                return _mapper.Map<KnightResponseDTO>(knight);
            }

            knight = await _knightRepository.GetCollectionById(id);
            if (knight == null)
                return null;

            await _redisService.SetValue(id, JsonConvert.SerializeObject(knight));
            return _mapper.Map<KnightResponseDTO>(knight);
        }

        public async Task<KnightResponseDTO> CreateKnight(KnightRequestDTO knightRequestDTO)
        {
            var knightEntity = _mapper.Map<KnightEntity>(knightRequestDTO);
            await _knightRepository.CreateCollection(knightEntity);

            await UpdateCache(knightEntity.Id, Constants.AllKnights, false, knightEntity);

            return _mapper.Map<KnightResponseDTO>(knightEntity);
        }

        public async Task UpdateKnight(string id, KnightRequestDTO knightRequestDTO)
        {
            var knightEntity = _mapper.Map<KnightEntity>(knightRequestDTO);
            await _knightRepository.UpdateCollection(id, knightEntity);

            await UpdateCache(id, Constants.AllKnights, false, knightEntity);
        }

        public async Task<KnightResponseDTO> UpdateKnightNickname(string id, NewNicknameRequestDTO newNicknameRequestDTO)
        {
            var knight = await _knightRepository.GetCollectionById(id);
            if (knight == null)
                return null;

            knight.Nickname = newNicknameRequestDTO.NewNickname;
            await _knightRepository.UpdateCollection(id, knight);

            var hash = knight.Deleted ? Constants.AllHeros : Constants.AllKnights;
            await UpdateCache(id, hash, false, knight);
            return _mapper.Map<KnightResponseDTO>(knight);
        }

        public async Task DeleteKnight(string id)
        {
            var knight = await _knightRepository.GetCollectionById(id);
            await _knightRepository.DeleteCollection(id);

            await _redisService.RemoveKey(id);
            await UpdateCache(id, Constants.AllKnights, true);
            await UpdateCache(id, Constants.AllHeros, false, knight);
        }

        public async Task<bool> UpdateKnightDeleted(string id)
        {
            var knight = await _knightRepository.GetCollectionById(id);

            if (knight == null || knight.Deleted)
                return false;

            knight.Deleted = true;
            await _knightRepository.UpdateCollection(id, knight);

            await UpdateCache(id, Constants.AllHeros, false, knight);
            await UpdateCache(id, Constants.AllKnights, true);
            await _redisService.RemoveKey(id);

            return true;
        }


        #region Private
        private async Task UpdateCache(string id, string hash, bool delete, KnightEntity? knight = null)
        {
            if (!delete)
                await _redisService.SetValue(id, JsonConvert.SerializeObject(knight));

            List<KnightEntity> knightsList;
            var knightsCache = await _redisService.GetValue(hash);

            if (knightsCache == null)
                knightsList = new List<KnightEntity>();
            else
                knightsList = JsonConvert.DeserializeObject<List<KnightEntity>>(knightsCache);

            await UpdateKnightListCache(id, hash, knightsList, knight, delete);
        }

        private async Task UpdateKnightListCache(string id, string hash, List<KnightEntity> knights, KnightEntity knight, bool delete)
        {
            var index = knights.FindIndex(k => k.Id == id);

            if (index != -1 && delete)
            {
                knights.RemoveAt(index);
            }
            else if (index != -1)
            {
                knights[index] = knight;
            }
            else
            {
                knights.Add(knight);
            }

            var serializedKnights = JsonConvert.SerializeObject(knights);
            await _redisService.SetValue(hash, serializedKnights);
        }
        #endregion Private
    }
}
