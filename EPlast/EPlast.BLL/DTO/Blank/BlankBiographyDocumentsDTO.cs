using System.ComponentModel.DataAnnotations;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Blank;

namespace EPlast.BLL.DTO.Blank
{
    public class BlankBiographyDocumentsDto
    {
        public int ID { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
