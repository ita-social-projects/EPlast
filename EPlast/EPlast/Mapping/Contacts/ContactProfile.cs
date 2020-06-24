using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactsViewModel, ContactDTO>().ReverseMap();
        }
    }
}
