using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class DegreeProfile:Profile
    {
        public DegreeProfile()
        {
            CreateMap<Degree, DegreeDTO>();
            CreateMap<DegreeDTO, Degree>();
            CreateMap<DegreeViewModel, DegreeDTO>();
            CreateMap<DegreeDTO, DegreeViewModel>();
        }
    }
}
