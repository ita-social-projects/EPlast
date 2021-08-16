﻿using System;

namespace EPlast.BLL.DTO.Region
{
    public class RegionDocumentDTO : IDocumentDTO
    {
            public int ID { get; set; }
            public DateTime? SubmitDate { get; set; }
            public string BlobName { get; set; }
            public string FileName { get; set; }
            public int RegionId { get; set; }
        
    }
}
