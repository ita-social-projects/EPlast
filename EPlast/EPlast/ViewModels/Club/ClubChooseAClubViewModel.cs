using System.Collections.Generic;

namespace EPlast.ViewModels.Club
{
    public class ClubChooseAClubViewModel
    {
        public IEnumerable<ClubViewModel> Clubs { get; set; }
        public string UserId { get; set; }
    }
}
