using AutoMapper;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.WebApi.Mapping.Precautions
{
    public class SuggestedUserProfile : Profile
    {
        public SuggestedUserProfile()
        {
            CreateMap<SuggestedUserDto, User>().ReverseMap();
        }
    }
}
