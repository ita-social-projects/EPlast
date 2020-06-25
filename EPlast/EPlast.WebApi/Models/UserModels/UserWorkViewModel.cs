using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserWorkViewModel
    {
        public int? PlaceOfWorkID { get; set; }
        public IEnumerable<WorkDTO> PlaceOfWorkList { get; set; }
        public int? PositionID { get; set; }
        public IEnumerable<WorkDTO> PositionList { get; set; }
    }
}
