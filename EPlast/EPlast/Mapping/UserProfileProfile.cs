﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class UserProfileProfile:Profile
    {
        public UserProfileProfile()
        {
            CreateMap<DataAccess.Entities.UserProfile, UserProfileDTO>().
                ForMember(x => x.Education, q => q.MapFrom(a=>a.Education)).
                ForMember(x => x.Degree, q => q.MapFrom(a => a.Degree)).
                ForMember(x => x.Work, q => q.MapFrom(a => a.Work)).
                ForMember(x => x.Religion, q => q.MapFrom(a => a.Religion)).
                ForMember(x => x.Gender, q => q.MapFrom(a => a.Gender)).
                ForMember(x => x.Nationality, q => q.MapFrom(a => a.Nationality)).
                ForMember(x => x.User, q => q.MapFrom(a => a.User));

            CreateMap<UserProfileDTO, DataAccess.Entities.UserProfile>().
                ForMember(x => x.Education, q => q.MapFrom(a => a.Education)).
                ForMember(x => x.Degree, q => q.MapFrom(a => a.Degree)).
                ForMember(x => x.Work, q => q.MapFrom(a => a.Work)).
                ForMember(x => x.Religion, q => q.MapFrom(a => a.Religion)).
                ForMember(x => x.Gender, q => q.MapFrom(a => a.Gender)).
                ForMember(x => x.Nationality, q => q.MapFrom(a => a.Nationality)).
                ForMember(x => x.User, q => q.MapFrom(a => a.User));
            
            CreateMap<UserProfileViewModel, UserProfileDTO>().
                ForMember(x => x.Education, q => q.MapFrom(a => a.Education)).
                ForMember(x => x.Degree, q => q.MapFrom(a => a.Degree)).
                ForMember(x => x.Work, q => q.MapFrom(a => a.Work)).
                ForMember(x => x.Religion, q => q.MapFrom(a => a.Religion)).
                ForMember(x => x.Gender, q => q.MapFrom(a => a.Gender)).
                ForMember(x => x.Nationality, q => q.MapFrom(a => a.Nationality)).
                ForMember(x => x.User, q => q.MapFrom(a => a.User));

            CreateMap<UserProfileDTO,UserProfileViewModel>().
                ForMember(x => x.Education, q => q.MapFrom(a => a.Education)).
                ForMember(x => x.Degree, q => q.MapFrom(a => a.Degree)).
                ForMember(x => x.Work, q => q.MapFrom(a => a.Work)).
                ForMember(x => x.Religion, q => q.MapFrom(a => a.Religion)).
                ForMember(x => x.Gender, q => q.MapFrom(a => a.Gender)).
                ForMember(x => x.Nationality, q => q.MapFrom(a => a.Nationality)).
                ForMember(x => x.User, q => q.MapFrom(a => a.User));
        }
    }
}
