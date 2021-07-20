using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class MethodicDocumentTableObject
    {
        public int Count { get; set; }
        public int Total { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string GoverningBody { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
    }
}
