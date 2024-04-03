using JwtAuthenticationBackend.Data;
using JwtAuthenticationBackend.Model;
using JwtAuthenticationBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationBackend.Controller
{
    [EnableCors("cors")]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        UserManager<IdentityUser> _usermanager; 
        IJwtHandler _jwtHandler;
        ILogger<LoginController> _logger;

        public LoginController(UserManager<IdentityUser>userManager, IJwtHandler jwtHandler, ILogger<LoginController> logger)
        {
            _usermanager = userManager;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

       
        [HttpGet]
        public void Get() {
        }

       
        [HttpPost] 
        public async Task<ActionResult<ResponseAuthentication>> Post([FromBody]RequestAuthentication request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || string.IsNullOrEmpty(request.UsernameOrEmail) || string.IsNullOrEmpty(request.Password)) 
            {
                return BadRequest("bad credential.");
            }

            try
            {
                var user=await _usermanager.FindByNameAsync(request.UsernameOrEmail);

                if (user == null)
                {
                    user=await _usermanager.FindByEmailAsync(request.UsernameOrEmail);
                }


                if (user != null)
                {
                    var result = await _usermanager.CheckPasswordAsync(user, request.Password);
                    if (result)
                    {
                        var respone = _jwtHandler.getJwtAuthentication(user);
                        if (respone != null)
                        {
                            _logger.LogInformation(string.Format("user '{0}' has logged in.", user.UserName));
                            return Ok(respone);
                        }
                        else
                        {
                            _logger.LogInformation("token is null!");
                        }
                    }
                }
                return BadRequest("bad credential.");
                
            }
            catch (Exception exp)
            {
                _logger.LogInformation(exp.Message);
                return BadRequest("bad credential.");
            }
        }
    }
}
