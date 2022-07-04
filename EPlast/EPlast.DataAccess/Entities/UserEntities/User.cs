using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.DataAccess.Entities
{
    public class User : IdentityUser
    {
        public string CityName { get; set; }
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

        [Display(Name = "По батькові")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,26})*$",
            ErrorMessage = "По батькові має містити тільки літери")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Поле по-батькові повинне складати від 2 до 25 символів")]
        public string FatherName { get; set; }
        [StringLength(18, MinimumLength = 18, ErrorMessage = "Номер телефону повинен містити 10 цифр")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        public DateTime RegistredOn { get; set; }
        public DateTime EmailSendedOnRegister { get; set; }
        public DateTime EmailSendedOnForgotPassword { get; set; }
        public string ImagePath { get; set; }
        public bool SocialNetworking { get; set; }
        public UserProfile UserProfile { get; set; }
        public int RegionId { get; set; } // Do not use this field anywhere, it is used only for newly registered users
        public ICollection<ConfirmedUser> ConfirmedUsers { get; set; }
        public ICollection<Approver> Approvers { get; set; }
        public ICollection<EventAdmin> Events { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
        public ICollection<CityAdministration> CityAdministrations { get; set; }
        public ICollection<ClubAdministration> ClubAdministrations { get; set; }
        public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<RegionAdministration> RegionAdministrations { get; set; }
        public ICollection<GoverningBodyAdministration> GoverningBodyAdministrations { get; set; }
        public ICollection<AnnualReport> CreatedAnnualReports { get; set; }
        public ICollection<AnnualReport> NewCityAdminAnnualReports { get; set; }
        public UserPlastDegree UserPlastDegrees { get; set; }
        public ICollection<UserMembershipDates> UserMembershipDates { get; set; }
        public ICollection<EducatorsStaff.EducatorsStaff> UsersKadras { get; set; }
        public ICollection<UserDistinction> UserDistinctions { get; set; }
        public ICollection<BlankBiographyDocuments> BlankBiographyDocuments { get; set; }
        public ICollection<UserRenewal> UserRenewals { get; set; }
        public ClubReportPlastDegrees ClubReportPlastDegrees { get; set; }
        public ClubReportCities ClubReportCities { get; set; }

        public User()
        {
            Approvers = new List<Approver>();
            ConfirmedUsers = new List<ConfirmedUser>();
        }
    }
}