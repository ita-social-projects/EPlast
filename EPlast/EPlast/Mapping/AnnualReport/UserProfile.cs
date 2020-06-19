using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using EPlast.ViewModels.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.Mapping.AnnualReport
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserViewModel, UserDTO>().ReverseMap();
        }
    }
}