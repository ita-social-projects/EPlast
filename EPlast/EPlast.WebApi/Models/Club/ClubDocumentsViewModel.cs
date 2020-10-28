using System;

namespace EPlast.WebApi.Models.Club
{
    public class ClubDocumentsViewModel
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public ClubDocumentTypeViewModel ClubDocumentType { get; set; }
        public int ClubId { get; set; }
    }
}
