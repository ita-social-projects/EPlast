using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.PrecautionsDTO
{
    public class SuggestedUserDto
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAvailable { get; set; }
    }
}
