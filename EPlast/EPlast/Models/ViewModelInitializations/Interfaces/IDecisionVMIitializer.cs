using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.Models.ViewModelInitializations.Interfaces
{
    public interface IDecisionVMIitializer
    {
        IEnumerable<SelectListItem> GetDecesionStatusTypes();
    }
}