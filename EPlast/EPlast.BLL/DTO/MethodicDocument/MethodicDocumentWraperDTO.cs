using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO
{
   public class MethodicDocumentWraperDTO
    {
        public MethodicDocumentDTO MethodicDocument { get; set; }
        public string FileAsBase64 { get; set; }
    }
}
