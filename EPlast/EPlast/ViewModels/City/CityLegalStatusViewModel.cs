using EPlast.Models.Enums;
using System;

namespace EPlast.ViewModels.City
{
    public class CityLegalStatusViewModel
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public CityLegalStatusType LegalStatusType { get; set; }
        public int CityId { get; set; }
    }
}