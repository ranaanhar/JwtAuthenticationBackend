using System;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationRequirement:IAuthorizationRequirement
{
    public CustomAuthorizationRequirement(){}
}
