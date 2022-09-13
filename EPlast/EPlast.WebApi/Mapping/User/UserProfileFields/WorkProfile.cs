using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels.UserProfileFields;

namespace EPlast.WebApi.Mapping.User.UserProfileFields
{
    public class WorkProfile : Profile
    {
        public WorkProfile()
        {
            CreateMap<WorkDto, WorkViewModel>().ReverseMap();
        }
    }
}
