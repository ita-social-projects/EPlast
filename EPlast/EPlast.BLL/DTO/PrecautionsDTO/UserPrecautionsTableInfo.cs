using System;
using System.Collections.Generic;
using System.Text;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.DTO.PrecautionsDTO
{
    public class UserPrecautionsTableInfo
    {
        public IEnumerable<UserPrecautionsTableObject> UserPrecautions { get; set; }
        public int Total { get; set; }
    }
}
