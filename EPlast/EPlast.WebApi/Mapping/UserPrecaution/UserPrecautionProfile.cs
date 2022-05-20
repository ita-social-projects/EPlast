using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.ExtensionMethods;
using EPlast.WebApi.Models.Precaution;

namespace EPlast.WebApi.Mapping.UserPrecaution
{
    public class UserPrecautionProfile : Profile
    {
        public UserPrecautionProfile()
        {
            CreateMap<UserPrecautionViewModel, UserPrecautionDTO>()
                .ForMember(vm => vm.Status, dto => dto.MapFrom(t => t.Status.GetDescription()))
                .ReverseMap();
        }
    }
}
