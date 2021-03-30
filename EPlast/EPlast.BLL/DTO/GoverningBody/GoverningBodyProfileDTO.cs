using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyProfileDTO
    {
        public GoverningBodyDTO GoverningBody { get; set; }
        public List<GoverningBodyDocumentsDTO> Documents { get; set; }
        public bool CanEdit { get; set; }
    }
}
