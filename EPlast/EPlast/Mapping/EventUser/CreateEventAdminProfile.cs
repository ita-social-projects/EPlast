using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class CreateEventAdminProfile : Profile
    {
        public CreateEventAdminProfile()
        {
            CreateMap<CreateEventAdminDTO, CreateEventAdminViewModel>().ReverseMap();
        }
    }
}
