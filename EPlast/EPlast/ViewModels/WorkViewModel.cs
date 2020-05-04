using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class WorkViewModel
    {
        public int? PlaceOfWorkID { get; set; }
        public IEnumerable<Work> PlaceOfWorkList { get; set; }
        public int? PositionID { get; set; }
        public IEnumerable<Work> PositionList { get; set; }
    }
}
