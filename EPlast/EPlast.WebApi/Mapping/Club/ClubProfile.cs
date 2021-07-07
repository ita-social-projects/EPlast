using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<DataAccessClub.Club, ClubDTO>().ReverseMap();
            CreateMap<ClubDTO, ClubViewModel>().ReverseMap();
            CreateMap<ClubProfileDTO, ClubViewModel>()
                .ForMember(r => r.Head, s => s.MapFrom(t => t.Head))
                .ForMember(r => r.Administration, s => s.MapFrom(t => t.Admins))
                .ForMember(r => r.Members, s => s.MapFrom(t => t.Members))
                .ForMember(r => r.Followers, s => s.MapFrom(t => t.Followers))
                .ForMember(r => r.Documents, s => s.MapFrom(t => t.Documents))
                .ForMember(r => r.ID, s => s.MapFrom(t => t.Club.ID))
                .ForMember(r => r.Name, s => s.MapFrom(t => t.Club.Name))
                .ForMember(r => r.PhoneNumber, s => s.MapFrom(t => t.Club.PhoneNumber))
                .ForMember(r => r.Email, s => s.MapFrom(t => t.Club.Email))
                .ForMember(r => r.ClubURL, s => s.MapFrom(t => t.Club.ClubURL))
                .ForMember(r => r.Description, s => s.MapFrom(t => t.Club.Description))
                .ForMember(r => r.Street, s => s.MapFrom(t => t.Club.Street))
                .ForMember(r => r.HouseNumber, s => s.MapFrom(t => t.Club.HouseNumber))
                .ForMember(r => r.OfficeNumber, s => s.MapFrom(t => t.Club.OfficeNumber))
                .ForMember(r => r.PostIndex, s => s.MapFrom(t => t.Club.PostIndex))
                .ForMember(r => r.Logo, s => s.MapFrom(t => t.Club.Logo))
                .ForMember(r => r.CanCreate, s => s.MapFrom(t => t.Club.CanCreate))
                .ForMember(r => r.CanJoin, s => s.MapFrom(t => t.Club.CanJoin))
                .ForMember(r => r.CanEdit, s => s.MapFrom(t => t.Club.CanEdit))
                .ForMember(r => r.MemberCount, s => s.MapFrom(t => t.Club.MemberCount))
                .ForMember(r => r.AdministrationCount, s => s.MapFrom(t => t.Club.AdministrationCount))
                .ForMember(r => r.FollowerCount, s => s.MapFrom(t => t.Club.FollowerCount))
                .ForMember(r => r.DocumentsCount, s => s.MapFrom(t => t.Club.DocumentsCount));


        }
    }
}