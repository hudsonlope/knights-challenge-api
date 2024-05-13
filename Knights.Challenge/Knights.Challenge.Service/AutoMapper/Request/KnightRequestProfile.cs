using AutoMapper;
using Knights.Challenge.Domain.DTOs.Request;
using Knights.Challenge.Domain.Entities;

namespace Knights.Challenge.Service.AutoMapper.Request
{
    public class KnightRequestProfile : Profile
    {
        public KnightRequestProfile()
        {
            CreateMap<KnightRequestDTO, KnightEntity>().ReverseMap();
        }
    }
}