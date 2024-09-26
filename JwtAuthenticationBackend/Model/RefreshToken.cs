using System;

namespace JwtAuthenticationBackend.Model;

public class RefreshToken
{
    public string? UserId { get; set; }
    public string? Token { get; set;}
    public string? Expiration { get; set; }
}
