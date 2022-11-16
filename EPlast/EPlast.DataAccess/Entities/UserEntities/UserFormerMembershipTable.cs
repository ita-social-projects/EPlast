using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class UserFormerMembershipTable
    {
        public int ID { get; set; }
        public DateTime? Entry { get; set; }
        public DateTime? End { get; set; }
    }
}
