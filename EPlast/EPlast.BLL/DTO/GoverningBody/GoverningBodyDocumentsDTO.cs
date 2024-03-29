﻿using System;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyDocumentsDto : IDocumentDto
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public int GoverningBodyDocumentTypeId { get; set; }
        public GoverningBodyDocumentTypeDto GoverningBodyDocumentType { get; set; }
        public int GoverningBodyId { get; set; }
    }
}
