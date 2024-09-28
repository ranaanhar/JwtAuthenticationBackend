using System;

namespace JwtAuthenticationBackend.Model;

public class RequestRefreshToken
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}

