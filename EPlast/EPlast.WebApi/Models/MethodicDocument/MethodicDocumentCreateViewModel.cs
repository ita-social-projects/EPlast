using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.MethodicDocument
{
    public class MethodicDocumentCreateViewModel
    {
        public IEnumerable<OrganizationDTO> Organizations { get; set; }
        public IEnumerable<SelectListItem> MethodicDocumentTypesItems { get; set; }
    }
}
