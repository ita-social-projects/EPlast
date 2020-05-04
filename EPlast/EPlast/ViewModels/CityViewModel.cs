using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class CityViewModel
    {
        public City City { get; set; }
        public CityAdministration CityHead { get; set; }
        public List<CityAdministration> CityAdmins { get; set; }
        public List<CityMembers> Members { get; set; }
        public List<CityMembers> Followers { get; set; }
        public List<CityDocuments> CityDoc { get; set; }

    }
}
