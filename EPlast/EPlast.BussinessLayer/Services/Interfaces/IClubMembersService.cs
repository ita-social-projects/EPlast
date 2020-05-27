using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.Club;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IClubMembersService
    {
        IEnumerable<DataAccess.Entities.ClubMembers> GetClubMembers(DataAccess.Entities.Club club, bool isApproved, int amount);
        IEnumerable<ClubMembersDTO> GetClubMembersDTO(ClubDTO club, bool isApproved, int amount);
        IEnumerable<DataAccess.Entities.ClubMembers> GetClubMembers(DataAccess.Entities.Club club, bool isApproved);
        IEnumerable<ClubMembersDTO> GetClubMembersDTO(ClubDTO club, bool isApproved);
        void ToggleIsApprovedInClubMembers(int memberId, int clubId);
        void AddFollower(int index, string userId);
    }
}
