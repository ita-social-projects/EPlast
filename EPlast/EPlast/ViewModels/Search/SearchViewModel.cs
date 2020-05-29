using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class SearchSurname
    {
        public IEnumerable<User> Users { get; set; }
        public string Surname { get; set; }

    }
}
