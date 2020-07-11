using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Interfaces.Jwt
{
    public interface IJwtService
    {
        string GenerateJWTToken(UserDTO userDTO);
    }
}
