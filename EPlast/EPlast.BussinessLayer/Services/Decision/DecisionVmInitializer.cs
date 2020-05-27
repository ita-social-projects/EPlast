﻿using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer
{
    public class DecisionVmInitializer : IDecisionVmInitializer
    {
        public IEnumerable<SelectListItem> GetDecesionStatusTypes() =>
            (from Enum decisionStatusType in Enum.GetValues(typeof(DecesionStatusType))
             select new SelectListItem
             {
                 Value = decisionStatusType.ToString(),
                 Text = decisionStatusType.GetDescription()
             }).ToList();
    }
}