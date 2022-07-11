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
            CreateMap<DataAccessClub.Club, ClubDto>().ReverseMap();
            CreateMap<ClubDto, ClubViewModel>().ReverseMap();
            CreateMap<ClubProfileDto, ClubViewModel>()
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
                .ForMember(r => r.Slogan, s => s.MapFrom(t => t.Club.Slogan))
                .ForMember(r => r.Logo, s => s.MapFrom(t => t.Club.Logo))
                .ForMember(r => r.CanJoin, s => s.MapFrom(t => t.Club.CanJoin))
                .ForMember(r => r.isActive, s => s.MapFrom(t => t.Club.IsActive))
                .ForMember(r => r.MemberCount, s => s.MapFrom(t => t.Club.MemberCount))
                .ForMember(r => r.AdministrationCount, s => s.MapFrom(t => t.Club.AdministrationCount))
                .ForMember(r => r.FollowerCount, s => s.MapFrom(t => t.Club.FollowerCount))
                .ForMember(r => r.DocumentsCount, s => s.MapFrom(t => t.Club.DocumentsCount));


        }
    }
}