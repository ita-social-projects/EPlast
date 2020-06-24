using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.Bussiness
{
    public interface IDecisionVmInitializer
    {
        IEnumerable<SelectListItem> GetDecesionStatusTypes();
    }
}