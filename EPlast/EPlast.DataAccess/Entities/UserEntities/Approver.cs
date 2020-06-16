using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class Approver
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public ConfirmedUser ConfirmedUser { get; set; }
    }
}
