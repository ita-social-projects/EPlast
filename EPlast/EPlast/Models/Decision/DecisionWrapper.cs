using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EPlast.Models
{
    public class DecisionWrapper
    {
        public Decision Decision { get; set; }
        public List<DecisionTarget> DecisionTargets { get; set; }
        public IFormFile File { get; set; }
        public string Filename { get; set; }
    }
}