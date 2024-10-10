using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtAuthenticationBackend.Authorization;

public class CustomAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public string CustomAuthorize { get; set; }="";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (string.IsNullOrEmpty(CustomAuthorize))
        {
            context.Result=new UnauthorizedResult();
            return;
        }

        var username=context.HttpContext.User.Identity!.Name;
        var claims=context.HttpContext.User.Claims;

    }
}
