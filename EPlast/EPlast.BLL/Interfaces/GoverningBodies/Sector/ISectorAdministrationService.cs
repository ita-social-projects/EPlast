using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorAdministrationService
    {
        /// <summary>
        /// Adds Administrator to specified Sector
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Sector Administration object</param>
        /// <returns>Sector Administration object</returns>
        Task<SectorAdministrationDto> AddSectorAdministratorAsync(SectorAdministrationDto sectorAdministrationDto);

        /// <summary>
        /// Edits Administrator of specified Sector
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Sector Administration object</param>
        /// <returns>Sector Administration object</returns>
        Task<SectorAdministrationDto> EditSectorAdministratorAsync(SectorAdministrationDto sectorAdministrationDto);

        /// <summary>
        /// Removes Administrator of specified Sector
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        Task RemoveAdministratorAsync(int adminId);

        /// <summary>
        /// Removes Administration roles of user of specified Sector
        /// </summary>
        /// <param name="adminId">The id of the user</param>
        Task RemoveAdminRolesByUserIdAsync(string userId);

        /// <summary>
        /// Returns administrations of giver user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>Sector Administrations DTO</returns>
        Task<IEnumerable<SectorAdministrationDto>> GetUserAdministrations(string userId);
    }
}
