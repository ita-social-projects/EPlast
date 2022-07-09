using System;

namespace EPlast.BLL.DTO.City
{
    public class CityDocumentsDto : IDocumentDto
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int CityDocumentTypeId { get; set; }
        public CityDocumentTypeDto CityDocumentType { get; set; }
        public int CityId { get; set; }
    }
}
