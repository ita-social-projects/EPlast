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
        UserDTO GetUserProfile(string userId);
        void Update(UserDTO user, IFormFile file, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId);
        Task<TimeSpan> CheckOrAddPlastunRole(string userId, DateTime registeredOn);
        IEnumerable<ConfirmedUserDTO> GetConfirmedUsers(UserDTO user);
        ConfirmedUserDTO GetClubAdminConfirmedUser(UserDTO user);
        ConfirmedUserDTO GetCityAdminConfirmedUser(UserDTO user);
        bool CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, ClaimsPrincipal user);
    }
}
