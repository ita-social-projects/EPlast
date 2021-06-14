using AutoMapper;
using EPlast.BLL.DTO.Region;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
   public class RegionUser : Profile
    {
        public RegionUser()
        {
            CreateMap<DatabaseEntities.User, RegionUserDTO>().ReverseMap();
        }
    }
}
