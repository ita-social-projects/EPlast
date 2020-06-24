﻿using EPlast.Bussiness.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.UserProfiles
{
    public interface IReligionService
    {
        Task<IEnumerable<ReligionDTO>> GetAllAsync();
    }
}
