using AutoMapper;
using HumSafar.API.DTOs.Request;
using HumSafar.BL.DTO_BL;

namespace TrendyKart.API.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserRequest, RegisterUserBL>().ReverseMap();
        }
    }
}
