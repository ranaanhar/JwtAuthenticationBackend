using System;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
    {
        throw new NotImplementedException();
    }
}
