using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class UpuDegreeProfile : Profile
    {
        public UpuDegreeProfile()
        {
            CreateMap<UpuDegree, UpuDegreeDto>().ReverseMap();
        }
    }
}
