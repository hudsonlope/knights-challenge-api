using AutoMapper;
using Knights.Challenge.Domain.DTOs.Response;
using Knights.Challenge.Domain.Entities;

namespace Knights.Challenge.Service.AutoMapper.Response
{
    public class KnightResponseProfile : Profile
    {
        public KnightResponseProfile()
        {
            CreateMap<KnightEntity, KnightResponseDTO>().ReverseMap();
        }
    }
}