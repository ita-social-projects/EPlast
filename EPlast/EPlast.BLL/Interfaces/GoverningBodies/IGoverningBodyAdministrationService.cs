﻿using EPlast.BLL.DTO.GoverningBody;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyAdministrationService
    {
        Task<GoverningBodyAdministrationDTO> AddGoverningBodyAdministrator(GoverningBodyAdministrationDTO governingBodyAdministrationDto);

        Task<GoverningBodyAdministrationDTO> EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO adminDto);

        Task RemoveAdministratorAsync(int adminId);
    }
}
