using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ConfirmedUserProfile:Profile
    {
        public ConfirmedUserProfile()
        {
            CreateMap<ConfirmedUser, ConfirmedUserDTO>().
                ForMember(x => x.User, q => q.MapFrom(a => a.User));
            CreateMap<ConfirmedUserDTO, ConfirmedUser>().
                ForMember(x => x.User, q => q.MapFrom(a => a.User));
            CreateMap<ConfirmedUserViewModel, ConfirmedUserDTO>().
                ForMember(x => x.User, q => q.MapFrom(a => a.User));
            CreateMap<ConfirmedUserDTO, ConfirmedUserViewModel>().
                ForMember(x => x.User, q => q.MapFrom(a => a.User));
        }
    }
}
