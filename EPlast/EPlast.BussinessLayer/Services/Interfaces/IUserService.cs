using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserProfile(string userId);
        IEnumerable<UserProfile> GetUserProfiles();
        void Update(UserDTO user, IFormFile file);
        void Delete(string user);
        Task<TimeSpan> CheckOrAddPlastunRole(string userId, DateTime registeredOn);
        Task<IEnumerable<ConfirmedUserDTO>> GetConfirmedUsers(UserDTO user);
        Task<ConfirmedUserDTO> GetClubAdminConfirmedUser(UserDTO user);
        Task<ConfirmedUserDTO> GetCityAdminConfirmedUser(UserDTO user);
        Task<bool> CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, ClaimsPrincipal user);
    }
}
