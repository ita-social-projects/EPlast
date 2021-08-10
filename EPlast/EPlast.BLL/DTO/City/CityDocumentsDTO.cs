using System;

namespace EPlast.BLL.DTO.City
{
    public class CityDocumentsDTO : IDocumentDTO
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int CityDocumentTypeId { get; set; }
        public CityDocumentTypeDTO CityDocumentType { get; set; }
        public int CityId { get; set; }
    }
}
