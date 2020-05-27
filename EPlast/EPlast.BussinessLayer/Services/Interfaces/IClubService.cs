using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.Club;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DataAccess = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IClubService
    {
        IEnumerable<DataAccess.Entities.Club> GetAllClubs();
        IEnumerable<ClubDTO> GetAllDTO();
        DataAccess.Entities.Club GetByIdWithDetails(int id);
        DataAccess.Entities.Club GetById(int id);
        ClubDTO GetByIdWithDetailsDTO(int id);
        ClubDTO GetByIdDTO(int id);
        void Update(ClubDTO club, IFormFile file);
        void Create(ClubDTO club, IFormFile file);
    }
}
