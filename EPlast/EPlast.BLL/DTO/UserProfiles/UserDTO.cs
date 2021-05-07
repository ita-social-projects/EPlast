using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.UserProfiles
{
    public class UserDTO : IdentityUser
    {
        [Display(Name = "Ім'я")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Ім'я повинне складати від 2 до 25 символів")]
        public string FirstName { get; set; }

        [Display(Name = "Прізвище")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "Прізвище має містити тільки літери")]
        [Required(ErrorMessage = "Поле прізвище є обов'язковим")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Прізвище повинне складати від 2 до 25 символів")]
        public string LastName { get; set; }

        [Display(Name = "По-батькові")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "По-батькові має містити тільки літери")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Поле по-батькові повинне складати від 2 до 25 символів")]
        public string FatherName { get; set; }

        [StringLength(18, MinimumLength = 18, ErrorMessage = "Номер телефону повинен містити 10 цифр")]
        [Required(ErrorMessage = "Поле номер телефону є обов'язковим")]
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
        public IEnumerable<ClubMembers> ClubMembers { get; set; }
        public IEnumerable<RegionAdministration> RegionAdministrations { get; set; }
        public IEnumerable<UserDistinctionDTO> UserDistinctions { get; set; }
        public UserMembershipDatesDTO UserMembershipDates { get; set; }
        public IEnumerable<UserPlastDegreeDTO> UserPlastDegrees { get; set; }
        public string CityName { get; set; }

        public UserDTO()
        {
            Approvers = new List<ApproverDTO>();
            ConfirmedUsers = new List<ConfirmedUserDTO>();
        }
    }
}
