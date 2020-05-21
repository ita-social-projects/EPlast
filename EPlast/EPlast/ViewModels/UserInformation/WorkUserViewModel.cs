using EPlast.ViewModels.UserInformation.UserProfile;
using System.Collections.Generic;

namespace EPlast.ViewModels.UserInformation
{
    public class WorkUserViewModel
    {
        public int? PlaceOfWorkID { get; set; }
        public IEnumerable<WorkViewModel> PlaceOfWorkList { get; set; }
        public int? PositionID { get; set; }
        public IEnumerable<WorkViewModel> PositionList { get; set; }
    }
}
