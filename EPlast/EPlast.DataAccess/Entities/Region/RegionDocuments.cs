using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
   public class RegionDocuments
    {
            public int ID { get; set; }
            public DateTime? SubmitDate { get; set; }
            public string BlobName { get; set; }
            public string FileName { get; set; }
            public int RegionId { get; set; }

    }
}
