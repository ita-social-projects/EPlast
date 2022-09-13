using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.DTO.Club
{
    public class ClubDocumentsDto : IDocumentDto
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int ClubDocumentTypeId { get; set; }
        public ClubDocumentTypeDto ClubDocumentType { get; set; }
        public int ClubId { get; set; }
    }
}
