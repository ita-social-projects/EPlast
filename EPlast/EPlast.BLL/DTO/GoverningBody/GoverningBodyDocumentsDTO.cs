using System;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyDocumentsDTO
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int CityDocumentTypeId { get; set; }
        public GoverningBodyDocumentTypeDTO CityDocumentType { get; set; }
        public int CityId { get; set; }
    }
}
