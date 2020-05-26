using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.Account
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
