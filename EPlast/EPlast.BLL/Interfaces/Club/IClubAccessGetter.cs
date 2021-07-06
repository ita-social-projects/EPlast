using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters
{
    public interface IClubAccessGetter
    {
        Task<IEnumerable<DatabaseEntities.Club>> GetClubs(string userId);
    }
}
