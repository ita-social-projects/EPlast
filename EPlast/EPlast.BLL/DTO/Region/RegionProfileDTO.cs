using EPlast.BLL.DTO.City;
using System;
using System.Collections.Generic;
using System.Text;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Region
{
    public class RegionProfileDTO
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string phoneNumber { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public int PostIndex { get; set; }
        public string City { get; set; }
        public bool CanEdit{ get; set; }
        public IEnumerable<RegionDocuments> Documents { get; set; }
        public int DocumentsCount { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
        public RegionsStatusTypeDTO Status { get; set; }
    }
}
