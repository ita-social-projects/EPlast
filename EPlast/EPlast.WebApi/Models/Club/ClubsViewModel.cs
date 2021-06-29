using EPlast.BLL.DTO.Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.Club
{
    public class ClubsViewModel
    {
        public ClubsViewModel(int page, int pageSize, IEnumerable<ClubDTO> сlubs, string clubName, bool isAdmin)
        {
            if (clubName == null)
            {
                Clubs = сlubs.Skip((page - 1) * pageSize).Take(pageSize);
                Total = сlubs.Count();
                CanCreate = isAdmin;
            }
            else
            {
                Clubs = from club in сlubs where club.Name.ToLower().Contains(clubName.ToLower()) select club;
                Total = Clubs.Count();
                CanCreate = isAdmin;
            }
        }

        public IEnumerable<ClubDTO> Clubs { get; set; }
        public int Total { get; set; }
        public bool CanCreate { get; set; }
    }
}
