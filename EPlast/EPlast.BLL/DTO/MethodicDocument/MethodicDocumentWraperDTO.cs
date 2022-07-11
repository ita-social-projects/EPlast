using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO
{
    public class MethodicDocumentWraperDto
    {
        public MethodicDocumentDto MethodicDocument { get; set; }
        public string FileAsBase64 { get; set; }
    }
}
