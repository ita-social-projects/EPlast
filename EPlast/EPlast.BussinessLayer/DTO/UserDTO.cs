using System;
using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BussinessLayer.DTO
{
    public class UserDTO:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public override string PhoneNumber { get; set; }
        public DateTime RegistredOn { get; set; }
        public DateTime EmailSendedOnRegister { get; set; }
        public DateTime EmailSendedOnForgotPassword { get; set; }
        public string ImagePath { get; set; }
        public bool SocialNetworking { get; set; }
        public UserProfileDTO UserProfile { get; set; }
        public ICollection<ConfirmedUserDTO> ConfirmedUsers { get; set; }
        public ICollection<ApproverDTO> Approvers { get; set; }
        public ICollection<EventAdmin> Events { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
        public ICollection<CityAdministration> CityAdministrations { get; set; }
        public ICollection<UnconfirmedCityMember> UnconfirmedCityMembers { get; set; }
        public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<RegionAdministration> RegionAdministrations { get; set; }
        public ICollection<AnnualReport> AnnualReports { get; set; }
        public ICollection<UserPlastDegree> UserPlastDegrees { get; set; }

        public UserDTO()
        {
            Approvers = new List<ApproverDTO>();
            ConfirmedUsers = new List<ConfirmedUserDTO>();
        }
    }
}
