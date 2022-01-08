using EPlast.BLL.DTO.AboutBase;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.AboutBase
{
    /// <summary>
    ///  Implements  operations for work with subsection gallery.
    /// </summary>
    public interface IAboutBasePicturesManager
    {
        /// <summary>
        /// Add pictures to gallery of specific subsection by subsection Id.
        /// </summary>
        /// <returns>List of added pictures.</returns>
        /// <param name="id">The Id of subsection</param>
        /// <param name="files">List of uploaded pictures</param>
        Task<IEnumerable<SubsectionPicturesDTO>> AddPicturesAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Delete picture by Id.
        /// </summary>
        /// <returns>Status code of the picture deleting operation.</returns>
        /// <param name="id">The Id of picture</param>
        Task<int> DeletePictureAsync(int id);

        /// <summary>
        /// Get pictures in Base64 format by subsection Id.
        /// </summary>
        /// <returns>List of pictures in Base64 format.</returns>
        /// <param name="subsectionId">The Id of subsection</param>
        Task<IEnumerable<SubsectionPicturesDTO>> GetPicturesInBase64(int subsectionId);
    }
}
