using JwtAuthenticationBackend.Model;
using JwtAuthenticationBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthenticationBackend.Controller
{
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IJwtHandler _jwtHandler;

        public RefreshTokenController(UserManager<ApplicationUser> userManager, ILogger<RefreshTokenController> logger, IJwtHandler jwtHandler)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtHandler = jwtHandler;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Get()
        {
            var result = new RequestRefreshToken { AccessToken = "token", RefreshToken = "refresh" };
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RequestRefreshToken request)
        {
            if (ModelState.IsValid)
            {

                var result = new RequestRefreshToken { AccessToken = request.AccessToken, RefreshToken = request.RefreshToken };
                if (request != null)
                {
                    var accessToken = request.AccessToken;
                    var refreshToken = request.RefreshToken;
                    if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
                    {
                        var responseAuthentication = await RefreshToken(accessToken, refreshToken);
                        if (responseAuthentication != null)
                        {
                            return Ok(responseAuthentication);
                        }
                    }
                }
            }
            return BadRequest();
        }



        /// <summary>
        /// validate access token and generate new access token and refresh token
        /// </summary>
        /// <param name="accessToken">string</param>
        /// <param name="refreshToken">string</param>
        /// <returns>ResponseAuthentication</returns>
        private async Task<ResponseAuthentication?> RefreshToken(string accessToken, string refreshToken)
        {
            var principals = _jwtHandler.GetClaimsPrincipal(accessToken);

            if (principals != null)
            {
                var identity = principals.Identity;
                if (identity != null)
                {
                    var username = identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        var user = await _userManager.FindByNameAsync(username!);
                        if (user != null)
                        {
                            //TODO check refresh token in refresh tokens chain
                            if (user.RefreshToken == refreshToken && user.RefreshTokenExpiration > DateTime.UtcNow)
                            {
                                var responseAuthentication = _jwtHandler.getJwtAuthentication(user);

                                if (responseAuthentication != null){
                                    user.RefreshToken=responseAuthentication!.RefreshToken;
                                    user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(Data.DataConstants.RefreshTokenExpiration);
                                    await _userManager.UpdateAsync(user);
                                    return responseAuthentication;
                                }
                            }
                        }
                    }
                }
           
            }
            return null;
        }
    }
}
