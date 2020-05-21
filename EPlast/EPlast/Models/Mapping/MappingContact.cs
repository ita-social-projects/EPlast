using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Models.Mapping
{
    public class MappingContact : Profile
    {
        public MappingContact()
        {
            CreateMap<ContactsViewModel, ContactDTO>();
            CreateMap<ContactDTO, ContactsViewModel>();
        }
    }
}
