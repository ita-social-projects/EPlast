﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.EventUser
{
    public class EventGeneralInfoViewModel
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
    }
}
