using System.Collections.Generic;
using EPlast.BLL.DTO;

namespace EPlast.WebApi.Models.Admin
{
    public class AdminTypeViewModel
    {
        public int ID { get; set; }
        public string AdminTypeName { get; set; }
        public IEnumerable<UserTableDto> Users { get; set; }
        public int Total { get; set; }
    }
}
