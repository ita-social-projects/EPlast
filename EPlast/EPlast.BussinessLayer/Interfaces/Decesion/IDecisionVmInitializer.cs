using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.BussinessLayer
{
    public interface IDecisionVmInitializer
    {
        IEnumerable<SelectListItem> GetDecesionStatusTypes();
    }
}