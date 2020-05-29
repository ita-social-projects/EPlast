using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.Account
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
