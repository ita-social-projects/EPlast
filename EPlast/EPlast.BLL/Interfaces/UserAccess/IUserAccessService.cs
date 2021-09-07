using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserAccess
{
    public interface IUserAccessService
    {
        Task<Dictionary<string, bool>> GetUserClubAccessAsync(int clubId,string userId, User user);
    }
}
