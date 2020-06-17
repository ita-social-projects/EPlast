using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    public class ClubAdministrationProfile : Profile
    {
        public ClubAdministrationProfile()
        {
            CreateMap<ClubAdministration, ClubAdministrationDTO>().ReverseMap();
        }
    }
}
