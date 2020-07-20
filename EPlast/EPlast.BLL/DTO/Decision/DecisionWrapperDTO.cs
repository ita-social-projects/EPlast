using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BLL.DTO
{
    public class DecisionWrapperDTO
    {
        public DecisionDTO Decision { get; set; }
        public IEnumerable<DecisionTargetDTO> DecisionTargets { get; set; }
        public string File { get; set; }
    }
}