using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using Microsoft.AspNetCore.Http;

namespace EPlast.BLL.Interfaces.AboutBase
{
    public interface IPicturesManager
    {
        /// <summary>
        /// Get pictures in Base64 format by subsection Id.
        /// </summary>
        /// <returns>List of pictures in Base64 format.</returns>
        /// <param name="id">The Id of subsection</param>
        Task<IEnumerable<SubsectionPicturesDto>> GetPicturesAsync(int id);

        /// <summary>
        /// Add pictures to gallery of specific subsection by subsection Id.
        /// </summary>
        /// <returns>List of added pictures.</returns>
        /// <param name="id">The Id of subsection</param>
        /// <param name="files">List of uploaded pictures</param>
        Task<IEnumerable<SubsectionPicturesDto>> FillSubsectionPicturesAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Delete picture by Id.
        /// </summary>
        /// <returns>Status code of the picture deleting operation.</returns>
        /// <param name="id">The Id of picture</param>
        Task<int> DeletePictureAsync(int id);
    }
}
