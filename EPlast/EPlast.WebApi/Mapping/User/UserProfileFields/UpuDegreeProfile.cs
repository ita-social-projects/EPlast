using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels.UserProfileFields;

namespace EPlast.WebApi.Mapping.User.UserProfileFields
{
    public class UpuDegreeProfile : Profile
    {
        public UpuDegreeProfile()
        {
            CreateMap<UpuDegreeDto, UpuDegreeViewModel>().ReverseMap();
        }
    }
}
