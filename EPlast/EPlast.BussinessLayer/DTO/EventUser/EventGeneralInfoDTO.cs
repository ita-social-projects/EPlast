using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class EventGeneralInfoDTO
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }

    }
}
