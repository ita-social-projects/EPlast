using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.BusinessLogicLayer
{
    public interface IDecisionVmInitializer
    {
        IEnumerable<SelectListItem> GetDecesionStatusTypes();
    }
}