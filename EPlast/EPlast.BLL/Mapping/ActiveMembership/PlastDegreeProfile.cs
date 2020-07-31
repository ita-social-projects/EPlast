using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.ActiveMembership
{
    public class PlastDegreeProfile : Profile
    {
        public PlastDegreeProfile()
        {
            CreateMap<PlastDergeeDTO, PlastDegree>().ReverseMap();
        }
    }
}
