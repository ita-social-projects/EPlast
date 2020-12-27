﻿using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityAccessService
    {
        Task<IEnumerable<CityDTO>> GetCitiesAsync(User user);
        Task<bool> HasAccessAsync(User user, int cityId);
    }
}