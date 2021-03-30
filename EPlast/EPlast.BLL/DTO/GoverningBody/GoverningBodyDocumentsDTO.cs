using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyDocumentsDTO
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int CityDocumentTypeId { get; set; }
        public GoverningBodyDocumentTypeDTO CityDocumentType { get; set; }
        public int CityId { get; set; }
    }
}
