using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
{
    //private readonly ILogger<CustomAuthorizationHandler> _logger;
    // public CustomAuthorizationHandler(Logger<CustomAuthorizationHandler>logger){
    //     _logger = logger;
    // }



    //Handle Requirement
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
    {
        //_logger.LogInformation("In Custom Authorization Handler");

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
