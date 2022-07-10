using System;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.Resources;

namespace EPlast.BLL.DTO
{
    public class UserTableDto
    {
        public ShortUserInformationDto User { get; set; }
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public GenderDto Gender { get; set; }
        public string Email { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Referal { get; set; }
        public UkraineOblasts Oblast { get; set; }
        public string UserPlastDegreeName { get; set; }
        public string UserRoles { get; set; }
        public string Comment { get; set; }
        public string UPUDegree { get; set; }
        public int UserSystemId { get; set; }
        public int? RegionId { get; set; }
        public int? CityId { get; set; }
        public int? ClubId { get; set; }
    }
}
