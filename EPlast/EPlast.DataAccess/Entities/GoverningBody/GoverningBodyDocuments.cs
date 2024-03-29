﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody
{
    public class GoverningBodyDocuments
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }

        [Required, MaxLength(64)]
        public string BlobName { get; set; }

        public string FileName { get; set; }

        [Required]
        public int GoverningBodyDocumentTypeId { get; set; }

        public GoverningBodyDocumentType GoverningBodyDocumentType { get; set; }

        [Required]
        public int GoverningBodyId { get; set; }

        public Organization GoverningBody { get; set; }
    }
}
