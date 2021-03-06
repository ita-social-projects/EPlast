﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Models
{
    public class FacebookUserInfo
    {
        public string AccessToken { get; set; }
        public string DataAccessExpirationTime { get; set; }
        public string Email { get; set; }
        public string ExpiresIn { get; set; }
        public string GraphDomain { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public object Picture { get; set; }
        public string UserId { get; set; }
        public string SignedRequest { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
    }
}
