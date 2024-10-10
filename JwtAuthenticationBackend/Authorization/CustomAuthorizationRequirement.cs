using System;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationRequirement : IAuthorizationRequirement
{
    public CustomAuthorizationRequirement() { }

    public  string AdminRole
    {
        get { return "Admin for example"; }
    }
}
