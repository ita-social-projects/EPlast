using AutoMapper;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.WebApi.Mapping.Precautions
{
    public class AvailableUserProfile : Profile
    {
        public AvailableUserProfile()
        {
            CreateMap<AvailableUserDTO, ShortUserInformationDTO>().ReverseMap();
            CreateMap<ShortUserInformationDTO, AvailableUserDTO>();
        }
    }
}
