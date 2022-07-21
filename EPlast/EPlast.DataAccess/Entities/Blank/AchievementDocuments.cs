using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Blank
{
    public class AchievementDocuments
    {
        public int ID { get; set; }
        public string BlobName { get; set; }

        public string FileName { get; set; }

        [Required]
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public User User { get; set; }
    }
}
