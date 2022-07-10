using System.Collections.Generic;
using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.WebApi.Models.MethodicDocument
{
    public class MethodicDocumentCreateViewModel
    {
        public IEnumerable<GoverningBodyDto> GoverningBodies { get; set; }
        public IEnumerable<SelectListItem> MethodicDocumentTypesItems { get; set; }
    }
}
