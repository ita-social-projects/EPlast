using System;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorDocumentsDTO : IDocumentDTO
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int SectorDocumentTypeId { get; set; }
        public SectorDocumentTypeDTO SectorDocumentType { get; set; }
        public int SectorId { get; set; }
    }
}
