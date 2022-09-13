using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.DTO.Region
{
    public class RegionProfileDto
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public int PostIndex { get; set; }
        public string City { get; set; }
        public UkraineOblasts Oblast { get; set; }
        public bool CanEdit{ get; set; }
        public IEnumerable<RegionDocuments> Documents { get; set; }
        public int DocumentsCount { get; set; }
        public IEnumerable<RegionAdministrationDto> Administration { get; set; }
        public IEnumerable<CityDto> Cities { get; set; }
        public RegionsStatusTypeDto Status { get; set; }
    }
}
