using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Interfaces.Jwt
{
    public interface IJwtService
    {
        /// <summary>
        /// Generting JWT token
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>Returns generated JWT token</returns>
        string GenerateJWTToken(UserDTO userDTO);
    }
}
