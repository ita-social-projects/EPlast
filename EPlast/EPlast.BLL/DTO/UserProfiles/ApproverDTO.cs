﻿using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO
{
    public class ApproverDTO
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public UserDTO User { get; set; }
        public ConfirmedUserDTO ConfirmedUser { get; set; }
    }
}
