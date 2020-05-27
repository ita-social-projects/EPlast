using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Club
{
    public class CLubViewModel
    {
        public int ID { get; set; }
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public IEnumerable<ClubMembersViewModel> ClubMembers { get; set; }
        public IEnumerable<ClubAdministrationViewModel> ClubAdministration { get; set; }
    }
}
