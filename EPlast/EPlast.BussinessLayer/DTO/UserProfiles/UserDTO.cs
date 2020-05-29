using System;
using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BussinessLayer.DTO.UserProfiles
{
    public class UserDTO : IdentityUser
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
        public IEnumerable<ConfirmedUserDTO> ConfirmedUsers { get; set; }
        public IEnumerable<ApproverDTO> Approvers { get; set; }
        public IEnumerable<EventAdmin> Events { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
        public IEnumerable<CityMembers> CityMembers { get; set; }
        public IEnumerable<CityAdministration> CityAdministrations { get; set; }
        public IEnumerable<UnconfirmedCityMember> UnconfirmedCityMembers { get; set; }
        public IEnumerable<ClubMembers> ClubMembers { get; set; }
        public IEnumerable<RegionAdministration> RegionAdministrations { get; set; }
        public IEnumerable<AnnualReport> AnnualReports { get; set; }
        public IEnumerable<UserPlastDegree> UserPlastDegrees { get; set; }

        public UserDTO()
        {
            Approvers = new List<ApproverDTO>();
            ConfirmedUsers = new List<ConfirmedUserDTO>();
        }
    }
}
