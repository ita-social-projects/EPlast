using AutoMapper;
using EPlast.BussinessLayer.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.City
{
    public class CityManagementProfile : Profile
    {
        public CityManagementProfile()
        {
            CreateMap<DatabaseEntities.CityManagement, CityManagementDTO>();
        }
    }
}