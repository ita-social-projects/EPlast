using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyTableProfile : Profile
    {
        public GoverningBodyTableProfile()
        {
            CreateMap<GoverningBodyAdministrationDTO, GoverningBodyTableViewModel>()
                .ForPath(x => x.Id, y => y.MapFrom(z => z.ID))
                .ForPath(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForPath(x => x.UserName, y => y.MapFrom(z => z.User.FirstName+" "+z.User.LastName))
                .ForPath(x => x.AdminType, y => y.MapFrom(z => z.AdminType.AdminTypeName))
                .ForPath(x => x.StartDate, y => y.MapFrom(z => z.StartDate))
                .ForPath(x => x.EndDate, y => y.MapFrom(z => z.EndDate))
                .ForPath(x => x.GovernBodyName, y => y.MapFrom(z => z.GoverningBody.GoverningBodyName))
                .ForPath(x => x.Status, y => y.MapFrom(z => z.Status));
        }
    }
}
