using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.User
{
    public class UserTableProfile : Profile
    {
        public UserTableProfile()
        {
            CreateMap<UserTableObject, UserTableDto>().ForPath(x => x.Gender.Name, d => d.MapFrom(y => y.Gender)).
                ForMember(u => u.UserPlastDegreeName, q => q.MapFrom(w => w.PlastDegree)).
                ForMember(u => u.UserRoles, q => q.MapFrom(w => w.Roles)).
                ReverseMap();
        }
    }
}
