using System;

namespace EPlast.WebApi.Models.City
{
    public class CityDocumentsViewModel
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public CityDocumentTypeViewModel CityDocumentType { get; set; }
        public int CityId { get; set; }
    }
}