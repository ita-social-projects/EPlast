namespace EPlast.DataAccess.Entities.AboutBase
{
    public class SubsectionPictures
    {
        public int SubsectionID { get; set; }
        public Subsection Subsection { get; set; }
        public int PictureID { get; set; }
        public Pictures Pictures { get; set; }
    }
}
