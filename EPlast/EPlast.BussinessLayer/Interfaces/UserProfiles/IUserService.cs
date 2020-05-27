using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.UserProfiles;
using Microsoft.AspNetCore.Http;

namespace EPlast.BussinessLayer.Interfaces.UserProfiles
{
    public interface IUserService
    {
        UserDTO GetUser(string userId);
        void Update(UserDTO user, IFormFile file, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId);
        Task<TimeSpan> CheckOrAddPlastunRole(string userId, DateTime registeredOn);
        IEnumerable<ConfirmedUserDTO> GetConfirmedUsers(UserDTO user);
        ConfirmedUserDTO GetClubAdminConfirmedUser(UserDTO user);
        ConfirmedUserDTO GetCityAdminConfirmedUser(UserDTO user);
        bool CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, ClaimsPrincipal user);
    }
}
