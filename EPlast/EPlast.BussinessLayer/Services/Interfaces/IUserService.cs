using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        UserDTO GetUserProfile(string userId);
        IEnumerable<UserProfile> GetUserProfiles();
        void Update(UserProfile user);
        void Delete(string user);
        Task<TimeSpan> CheckOrAddPlastunRole(string userId, DateTime registeredOn);
    }
}
