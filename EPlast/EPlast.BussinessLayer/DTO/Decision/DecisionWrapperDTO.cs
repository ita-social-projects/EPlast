using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO
{
    public class DecisionWrapperDTO
    {
        public DecisionDTO Decision { get; set; }
        public List<DecisionTargetDTO> DecisionTargets { get; set; }
        public IFormFile File { get; set; }
        public string Filename { get; set; }
    }
}