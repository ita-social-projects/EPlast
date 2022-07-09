using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.AnnualReport
{
    public class MembersStatisticProfile : Profile
    {
        public MembersStatisticProfile()
        {
            CreateMap<MembersStatistic, MembersStatisticDto>().ReverseMap();
        }
    }
}