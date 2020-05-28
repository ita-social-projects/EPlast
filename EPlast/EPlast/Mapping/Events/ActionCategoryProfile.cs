using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping.Events
{
    public class ActionCategoryProfile : Profile
    {
        public ActionCategoryProfile()
        {
            CreateMap<EventCategoryDTO, EventCategoryViewModel>().ReverseMap();
        }
    }


}
