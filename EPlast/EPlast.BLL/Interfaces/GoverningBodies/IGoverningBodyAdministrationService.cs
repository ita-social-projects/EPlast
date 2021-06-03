using EPlast.BLL.DTO.GoverningBody;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyAdministrationService
    {
        /// <summary>
        /// Adds Administrator to specified Governing Body
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Governing Body Administration object</param>
        /// <returns>Governing Body Administration object</returns>
        Task<GoverningBodyAdministrationDTO> AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO governingBodyAdministrationDto);

        /// <summary>
        /// Edits Administrator of specified Governing Body
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Governing Body Administration object</param>
        /// <returns>Governing Body Administration object</returns>
        Task<GoverningBodyAdministrationDTO> EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDTO governingBodyAdministrationDto);

        /// <summary>
        /// Removes Administrator of specified Governing Body
        /// </summary>
        /// <param name="adminId">Governing Body Administration object</param>
        Task RemoveAdministratorAsync(int adminId);
    }
}
