using EPlast.BLL.DTO;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.Admin
{
    public class AdminTypeViewModel
    {
        public int ID { get; set; }
        public string AdminTypeName { get; set; }
        public IEnumerable<UserTableDTO> Users { get; set; }
        public int Total { get; set; }
    }
}
