using System;

namespace EPlast.WebApi.Models.GoverningBody.Sector
{
    public class SectorDocumentsViewModel
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public SectorDocumentTypeViewModel SectorDocumentType { get; set; }
        public int SectorId { get; set; }
    }
}