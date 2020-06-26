using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.DataAccess.Entities;

namespace EPlast.Mapping
{
    public class AdminTypeProfile : Profile
    {
        public AdminTypeProfile()
        {
            CreateMap<AdminType, AdminTypeDTO>().ReverseMap();
        }
    }
}
