using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Jwt
{
    public interface IJwtService
    {
        /// <summary>
        /// Generates JWT token for further Bearer authentication
        /// </summary>
        /// <param name="user">Authenticated user</param>
        /// <returns>JWT token</returns>
        Task<string> GenerateJWTTokenAsync(UserDTO userDTO);

        /// <summary>
        /// Generates JWT token for further Bearer authentication
        /// </summary>
        /// <param name="user">Authenticated user</param>
        /// <returns>JWT token</returns>
        Task<string> GenerateJWTTokenAsync(User user);
    }
}
