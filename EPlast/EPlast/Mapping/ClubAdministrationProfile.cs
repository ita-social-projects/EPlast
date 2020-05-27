using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;

namespace EPlast.Mapping
{
    public class ClubAdministrationProfile : Profile
    {
        public ClubAdministrationProfile()
        {
            CreateMap<ClubAdministration, ClubAdministrationDTO>().ReverseMap();
            CreateMap<ClubAdministrationViewModel, ClubAdministrationDTO>().ReverseMap();
        }
    }
}
