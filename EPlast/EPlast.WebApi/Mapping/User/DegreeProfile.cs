using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels.UserProfileFields;

namespace EPlast.WebApi.Mapping.User
{
    public class DegreeProfile : Profile
    {
        public DegreeProfile()
        {
            CreateMap<DegreeDTO, DegreeViewModel>().ReverseMap();
        }
    }
}
