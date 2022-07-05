using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubAdministrationProfile : Profile
    {
        public ClubAdministrationProfile()
        {
            CreateMap<ClubAdministrationDto, ClubAdministrationViewModel>().ReverseMap();
        }
    }
}
