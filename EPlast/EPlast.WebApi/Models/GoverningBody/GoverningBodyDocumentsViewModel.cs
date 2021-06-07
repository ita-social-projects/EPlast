using System;

namespace EPlast.WebApi.Models.GoverningBody
{
    public class GoverningBodyDocumentsViewModel
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public GoverningBodyDocumentTypeViewModel GoverningBodyDocumentType { get; set; }
        public int GoverningBodyId { get; set; }
    }
}
