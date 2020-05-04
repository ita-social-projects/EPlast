using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class ClubViewModel
    {
        public Club Club { get; set; }
        public User ClubAdmin { get; set; }
        public List<ClubMembers> Members { get; set; }
        public List<ClubMembers> Followers { get; set; }
    }
}
