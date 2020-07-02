using System;

namespace EPlast.BLL.DTO.City
{
    public class CityDocumentsDTO
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string DocumentURL { get; set; }
        public CityDocumentTypeDTO CityDocumentType { get; set; }
    }
}
