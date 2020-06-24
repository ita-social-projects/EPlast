using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.BusinessLogicLayer.DTO.Admin;
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
