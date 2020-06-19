using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubAdministrationProfile : Profile
    {
        public ClubAdministrationProfile()
        {
            CreateMap<ClubAdministration, ClubAdministrationDTO>().ReverseMap();
            CreateMap<ClubAdministrationDTO, ClubAdministrationViewModel>().ReverseMap();
        }
    }
}
