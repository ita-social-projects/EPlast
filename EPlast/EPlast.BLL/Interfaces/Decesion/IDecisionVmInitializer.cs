using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.BLL
{
    public interface IDecisionVmInitializer
    {
        IEnumerable<SelectListItem> GetDecesionStatusTypes();
    }
}