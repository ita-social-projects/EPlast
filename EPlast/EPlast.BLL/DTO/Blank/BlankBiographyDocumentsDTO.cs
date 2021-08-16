using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Blank;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Blank
{
   public class BlankBiographyDocumentsDTO
    {
        public int ID { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
