using System.Collections.Generic;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyProfileDTO
    {
        public GoverningBodyDTO GoverningBody { get; set; }
        public IEnumerable<GoverningBodyDocumentsDTO> Documents { get; set; }
    }
}
