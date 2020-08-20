﻿using EPlast.BLL.DTO.UserProfiles;
using System;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class KadrasDTO
    {
        public int ID { get; set; }

        public string UserId { get; set; }
        public UserDTO user { get; set; }
        public int KVTypesID { get; set; }
        public KVTypeDTO kvTypes { get; set; }

        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }

        public string BasisOfGranting { get; set; }

        public string Link { get; set; }
    }
}
