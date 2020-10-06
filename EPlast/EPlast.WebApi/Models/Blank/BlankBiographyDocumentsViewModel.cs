using EPlast.DataAccess.Entities.Blank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.Blank
{
    public class BlankBiographyDocumentsViewModel
    {
        public int ID { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public BlankBiographyDocumentsTypeViewModel BlankBiographyDocumentsTypeViewModel { get; set; }
        public int UserId { get; set; }
    }
}
