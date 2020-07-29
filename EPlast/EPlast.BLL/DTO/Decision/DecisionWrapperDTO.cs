using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BLL.DTO
{
    public class DecisionWrapperDTO
    {
        public DecisionDTO Decision { get; set; }
        public string FileAsBase64 { get; set; }
    }
}