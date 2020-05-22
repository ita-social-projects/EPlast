using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactsViewModel, ContactDTO>();
            CreateMap<ContactDTO, ContactsViewModel>();
        }
    }
}
