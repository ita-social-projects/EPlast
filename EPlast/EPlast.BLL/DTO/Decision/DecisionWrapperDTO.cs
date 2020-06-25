using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BLL.DTO
{
    public class DecisionWrapperDTO
    {
        public DecisionDTO Decision { get; set; }
        public IEnumerable<DecisionTargetDTO> DecisionTargets { get; set; }
        public IFormFile File { get; set; }
        public string Filename { get; set; }
    }
}