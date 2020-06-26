using AutoMapper;
using EPlast.BLL.DTO.Events;
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
