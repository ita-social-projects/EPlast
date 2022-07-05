using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityViewModel, CityDTO>().ReverseMap();
            CreateMap<CityProfileDTO, CityViewModel>()
                .ForMember(r => r.Head, s => s.MapFrom(t => t.Head))
                .ForMember(r => r.Administration, s => s.MapFrom(t => t.Admins))
                .ForMember(r => r.Members, s => s.MapFrom(t => t.Members))
                .ForMember(r => r.Followers, s => s.MapFrom(t => t.Followers))
                .ForMember(r => r.Documents, s => s.MapFrom(t => t.Documents))
                .ForMember(r => r.ID, s => s.MapFrom(t => t.City.ID))
                .ForMember(r => r.Name, s => s.MapFrom(t => t.City.Name))
                .ForMember(r => r.PhoneNumber, s => s.MapFrom(t => t.City.PhoneNumber))
                .ForMember(r => r.Email, s => s.MapFrom(t => t.City.Email))
                .ForMember(r => r.CityURL, s => s.MapFrom(t => t.City.CityURL))
                .ForMember(r => r.Description, s => s.MapFrom(t => t.City.Description))
                .ForMember(r => r.Address, s => s.MapFrom(t => t.City.Address))
                .ForMember(r => r.Level, s => s.MapFrom(t => t.City.Level))
                .ForMember(r => r.Logo, s => s.MapFrom(t => t.City.Logo))
                .ForMember(r => r.Region, s => s.MapFrom(t => t.City.Region))
                .ForMember(r => r.CanCreate, s => s.MapFrom(t => t.City.CanCreate))
                .ForMember(r => r.CanJoin, s => s.MapFrom(t => t.City.CanJoin))
                .ForMember(r => r.CanEdit, s => s.MapFrom(t => t.City.CanEdit))
                .ForMember(r => r.IsActive, s => s.MapFrom(t => t.City.IsActive ))
                .ForMember(r => r.MemberCount, s => s.MapFrom(t => t.City.MemberCount))
                .ForMember(r => r.FollowerCount, s => s.MapFrom(t => t.City.FollowerCount))
                .ForMember(r => r.AdministrationCount, s => s.MapFrom(t => t.City.AdministrationCount))  
                .ForMember(r => r.DocumentsCount, s => s.MapFrom(t => t.City.DocumentsCount))
                .ForMember(r => r.Oblast, s => s.MapFrom(t => t.City.Oblast));

        }
    }
}