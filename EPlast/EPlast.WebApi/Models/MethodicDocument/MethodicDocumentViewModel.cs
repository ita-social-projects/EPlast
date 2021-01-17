using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.MethodicDocument
{
    public class MethodicDocumentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Organization { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public string FileName { get; set; }
    }
}
