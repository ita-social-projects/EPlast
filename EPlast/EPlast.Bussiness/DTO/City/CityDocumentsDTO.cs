using System;

namespace EPlast.BusinessLogicLayer.DTO.City
{
    public class CityDocumentsDTO
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string DocumentURL { get; set; }
        public CityDocumentTypeDTO CityDocumentType { get; set; }
        public CityDTO City { get; set; }
    }
}
