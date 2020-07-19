using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserWorkViewModel
    {
        public int? PlaceOfWorkID { get; set; }
        public IEnumerable<WorkViewModel> PlaceOfWorkList { get; set; }
        public int? PositionID { get; set; }
        public IEnumerable<WorkViewModel> PositionList { get; set; }
    }
}
