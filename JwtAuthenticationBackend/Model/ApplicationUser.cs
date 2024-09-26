using System;
using Microsoft.AspNetCore.Identity;

namespace JwtAuthenticationBackend.Model;

public class ApplicationUser:IdentityUser
{
        public string? RefreshToken { get; set;}
        public DateTime RefreshTokenExpiration { get; set; }
}
