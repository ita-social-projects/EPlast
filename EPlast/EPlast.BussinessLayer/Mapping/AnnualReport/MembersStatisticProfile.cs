using AutoMapper;
using EPlast.BussinessLayer.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class MembersStatisticProfile : Profile
    {
        public MembersStatisticProfile()
        {
            CreateMap<DatabaseEntities.MembersStatistic, MembersStatisticDTO>().ReverseMap();
        }
    }
}