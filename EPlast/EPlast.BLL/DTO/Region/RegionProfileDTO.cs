using EPlast.BLL.DTO.City;
using System;
using System.Collections.Generic;
using System.Text;

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
        public IEnumerable<RegionDocumentDTO> Documents { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<CityDTO> Cities { get; set; }
    }
}
