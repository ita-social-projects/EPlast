using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BussinessLayer.ExtensionMethods;

namespace EPlast.Models.ViewModelInitializations
{
    public class DecisionVMIitializer : IDecisionVMIitializer
    {
        public IEnumerable<SelectListItem> GetDecesionStatusTypes()
        {
            var decisionStatusTypesSLI = new List<SelectListItem>();
            foreach (Enum decisionStatusType in Enum.GetValues(typeof(DecesionStatusType)))
            {
                decisionStatusTypesSLI.Add(new SelectListItem
                {
                    Value = decisionStatusType.ToString(),
                    Text = decisionStatusType.GetDescription()
                });
            }
            return decisionStatusTypesSLI;
        }
    }
}