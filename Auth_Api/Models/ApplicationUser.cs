﻿using Microsoft.AspNetCore.Identity;

namespace Auth_Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
