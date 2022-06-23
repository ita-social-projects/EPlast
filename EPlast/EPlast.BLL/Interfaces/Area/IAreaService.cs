using EPlast.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAreaService
    {
        /// <summary>
        /// Method to get all areas
        /// </summary>
        /// <returns>All areas</returns>
        Task<IEnumerable<AreaDTO>> GetAllAsync();

        /// <summary>
        /// Method to get one area
        /// </summary>
        /// <param name="id">Area identification number</param>
        /// <returns>Area model</returns>
        /// <exception cref="System.NullReferenceException">Thrown when area doesn't exist</exception>
        Task<AreaDTO> GetByIdAsync(int id);
    }
}