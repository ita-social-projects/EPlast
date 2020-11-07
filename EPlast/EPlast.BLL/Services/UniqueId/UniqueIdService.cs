using EPlast.BLL.Interfaces;
using System;

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
