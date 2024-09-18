using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
{
    public string CustomrAuthorize { get; set; }="";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (string.IsNullOrEmpty(CustomrAuthorize))
        {
            context.Result=new UnauthorizedResult();
            return;
        }

        var username=context.HttpContext.User.Identity!.Name;
        var claims=context.HttpContext.User.Claims;
    }
}
