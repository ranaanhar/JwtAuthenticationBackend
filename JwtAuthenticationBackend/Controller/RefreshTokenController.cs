using JwtAuthenticationBackend.Model;
using JwtAuthenticationBackend.Service;
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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("request for refresh token.");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestRefreshToken request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(nameof(request));
            }
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(nameof(accessToken));
            }

            //TODO Implement Validate Refresh Token

            try
            {
                var principal = _jwtHandler.GetClaimsPrincipal(accessToken);
                if (principal == null)
                {
                    return BadRequest(nameof(accessToken));
                }

                //TODO check this code
                var identity = principal.Identity;
                if (identity != null)
                {
                    var username = identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        var user = await _userManager.FindByNameAsync(username);

                        if (user != null)
                        {
                            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiration < DateTime.UtcNow)
                            {
                                //refresh token is invalid ==> ??
                                return BadRequest();
                            }

                            //create new token.
                        }
                    }
                }
            }
            catch (SecurityTokenException exp)
            {
                _logger.LogInformation(exp, exp.Message);
            }
            catch (System.Exception exp)
            {
                _logger.LogInformation(exp, exp.Message);
            }


            return BadRequest();
        }
    }
}
