using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.Models.Jwt
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
