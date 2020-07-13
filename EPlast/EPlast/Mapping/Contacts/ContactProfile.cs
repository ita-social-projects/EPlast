using AutoMapper;
using EPlast.ViewModels;
using EPlast.BLL.DTO.Account;

namespace EPlast.Mapping
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactsViewModel, ContactsDto>().ReverseMap();
        }
    }
}
