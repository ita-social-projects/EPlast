namespace EPlast.BLL.DTO.AboutBase
{
    public class SubsectionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int SectionId { get; set; }
        public SectionDto Section { get; set; }

        public string Description { get; set; }

        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
    }
}
