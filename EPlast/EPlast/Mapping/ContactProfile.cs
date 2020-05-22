using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
