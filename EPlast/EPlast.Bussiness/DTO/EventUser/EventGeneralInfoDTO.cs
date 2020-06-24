using System;

namespace EPlast.Bussiness.DTO.EventUser
{
    public class EventGeneralInfoDTO
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }

    }
}
