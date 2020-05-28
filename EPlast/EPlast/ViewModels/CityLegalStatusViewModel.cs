using EPlast.Models.Enums;
using EPlast.ViewModels.City;
using System;

namespace EPlast.ViewModels
{
    public class CityLegalStatusViewModel
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public CityLegalStatusType LegalStatusType { get; set; }

        public int CityId { get; set; }
        public CityViewModel City { get; set; }

        public CityMembersViewModel CityManagement { get; set; }
    }
}