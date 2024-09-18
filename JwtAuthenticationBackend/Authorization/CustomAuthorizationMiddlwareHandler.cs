using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationMiddlwareHandler : IAuthorizationMiddlewareResultHandler
{
    public readonly AuthorizationMiddlewareResultHandler defaultHandler=new();
    public async Task HandleAsync(RequestDelegate next,
     HttpContext context, 
     AuthorizationPolicy policy,
      PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)// && authorizeResult.AuthorizationFailure!.FailedRequirements.OfType<Show404Requirement>().Any())
        {
            context.Response.StatusCode=StatusCodes.Status403Forbidden;
            return;
        }
        //throw new NotImplementedException();
        await defaultHandler.HandleAsync(next, context,policy,authorizeResult);
    }

}
    public class Show404Requirement:IAuthorizationRequirement
    {
        //public string Description { get; set;}
    }
