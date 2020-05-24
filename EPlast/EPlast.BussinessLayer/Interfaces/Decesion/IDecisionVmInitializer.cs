using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.BussinessLayer
{
    public interface IDecisionVmInitializer
    {
        IEnumerable<SelectListItem> GetDecesionStatusTypes();
    }
}