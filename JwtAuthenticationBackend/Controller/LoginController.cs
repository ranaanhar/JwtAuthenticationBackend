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
        readonly UserManager<ApplicationUser> _userManager;
        readonly IJwtHandler _jwtHandler;
        readonly ILogger<LoginController> _logger;

        public LoginController(UserManager<ApplicationUser> userManager, IJwtHandler jwtHandler, ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }


        [HttpGet]
        public void Get()
        {
            //TODO Fix This
            throw new NotImplementedException();
        }


        [HttpPost]
        public async Task<ActionResult<ResponseAuthentication>> Post([FromBody] RequestAuthentication request)
        {
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
                var user = await _userManager.FindByNameAsync(request.UsernameOrEmail);

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
                }


                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, request.Password);
                    if (result)
                    {
                        var response = _jwtHandler.getJwtAuthentication(user);
                        if (response != null)
                        {
                            await SaveRefreshToken(user, response.RefreshToken);
                            _logger.LogInformation("user '{0}' has logged in.", user.UserName);
                            return Ok(response);
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
                _logger.LogInformation(exp, exp.Message);
                return BadRequest("bad credential.");
            }
        }


        //save refresh token
        /// <summary>
        /// Save Refresh Token To Database
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <param name="token">string</param>
        /// <returns></returns>
        private async Task SaveRefreshToken(ApplicationUser user, string? token)
        {
            if (user == null || string.IsNullOrEmpty(token))
            {
                return;
            }

            user.RefreshToken = token;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(Data.DataConstants.RefreshTokenExpiration);
            await _userManager.UpdateAsync(user);
        }
    }


}
