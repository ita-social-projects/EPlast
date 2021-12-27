using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.WebApi.Models.GoverningBody.Sector;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorTableProfile : Profile
    {
        public SectorTableProfile()
        {
            CreateMap<SectorAdministrationDTO, SectorTableViewModel>()
                .ForPath(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForPath(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForPath(x => x.UserName, y => y.MapFrom(z => z.User.FirstName + " " + z.User.LastName))
                .ForPath(x => x.AdminType, y => y.MapFrom(z => z.AdminType.AdminTypeName))
                .ForPath(x => x.StartDate, y => y.MapFrom(z => z.StartDate))
                .ForPath(x => x.EndDate, y => y.MapFrom(z => z.EndDate))
                .ForPath(x => x.SectorName, y => y.MapFrom(z => z.Sector.Name))
                .ForPath(x => x.Status, y => y.MapFrom(z => z.Status));
        }
    }
}
