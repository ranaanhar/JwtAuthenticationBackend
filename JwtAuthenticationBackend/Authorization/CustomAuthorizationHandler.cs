using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
{
    //Handle Requirement
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
    {
        var role=context.User.Claims.FirstOrDefault(c => c.Type==ClaimTypes.Role);
        if (role!=null){
            if (role.Value==requirement.AdminRole)
            {
                //_logger.LogInformation("minimum requirement satisfied.");
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
