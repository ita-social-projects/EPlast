using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class CreateEventAdminDTO
    {
        public string UserID { get; set; }
        public UserDTO User { get; set; }
    }
}
