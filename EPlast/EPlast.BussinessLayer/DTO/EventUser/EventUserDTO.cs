﻿using System;
using System.Collections.Generic;
using System.Text;
using EPlast.BussinessLayer.DTO.UserProfiles;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class EventUserDTO
    {
        public UserDTO User { get; set; }
        public ICollection<EventGeneralInfoDTO> PlanedEvents { get; set; }
        public ICollection<EventGeneralInfoDTO> CreatedEvents { get; set; }
        public ICollection<EventGeneralInfoDTO> VisitedEvents { get; set; }
    }
}
