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
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult Get()
        {
            //TODO Fix this
            return BadRequest();
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
                        _logger.LogInformation("user is :{0}",user.UserName);
                        var response = _jwtHandler.getJwtAuthentication(user);
                        _logger.LogInformation("response : {0}",response!.ToString());
                        if (response != null)
                        {
                            await SaveRefreshToken(user, response.RefreshToken);
                            _logger.LogInformation("user '{0}' has logged in:\n ACC_TOK:{1} \n REF_TOK:{2}", user.UserName,response.Token,response.RefreshToken);
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
