using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EPlast.BLL.DTO
{
    public class DecisionWrapperDto
    {
        public DecisionDto Decision { get; set; }
        public string FileAsBase64 { get; set; }
    }
}