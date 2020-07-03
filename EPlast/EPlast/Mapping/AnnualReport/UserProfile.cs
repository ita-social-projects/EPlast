using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.ViewModels.AnnualReport;

namespace EPlast.Mapping.AnnualReport
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DataAccess.Entities.User, UserDTO>().ReverseMap();
            CreateMap<UserViewModel, UserDTO>().ReverseMap();
        }
    }
}