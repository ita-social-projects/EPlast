using EPlast.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Services
{
    public class UniqueIdService : IUniqueIdService
    {
        public Guid GetUniqueId()
        {
            return Guid.NewGuid();
        }
    }
}
