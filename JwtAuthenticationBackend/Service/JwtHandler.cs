using JwtAuthenticationBackend.Model;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationBackend.Service
{
    public class JwtHandler:IJwtHandler
    {
        ILogger<JwtHandler> _logger;
        IConfiguration _configuration;
        const int ExpirationTime = 10;
        public JwtHandler(IConfiguration configuration, ILogger<JwtHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public ResponseAuthentication? getJwtAuthentication(IdentityUser user)
        {
            try
            {
                DateTime expire = DateTime.UtcNow.AddMinutes(ExpirationTime);
                if (user == null) {
                    throw new ArgumentNullException("user null received!"); 
                }
                var handler = new JwtSecurityTokenHandler();
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:issuer"],
                    audience: _configuration["JWT:audience"],
                    claims:getClaims(user),
                    notBefore:DateTime.UtcNow,
                    expires:expire,
                    signingCredentials:getSigninCredentials()
                    );
                getExpires(expire);
                return new ResponseAuthentication() { Expiration = getExpires(expire) ,Token=handler.WriteToken(token)};
            }
            catch (Exception exp)
            {
                _logger.LogInformation(string.Format(exp.Message));
            }
            return null;
        }

        private SigningCredentials getSigninCredentials()
        {
            var key = _configuration["JWT:key"]!;
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),SecurityAlgorithms.HmacSha256);
        }

        private Claim[] getClaims(IdentityUser user) {
            return [
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString()),
                //other claims
            ];
        }

        private string getExpires(DateTime time) {
            
                var timefrom1970 = time - new DateTime(1970, 1, 1);
                var timeToMilliseconds = Math.Ceiling(timefrom1970.TotalMilliseconds);
                _logger.LogInformation($"Expires: {timeToMilliseconds}");
                return timeToMilliseconds.ToString();
        }
    }

    public interface IJwtHandler {
        public ResponseAuthentication? getJwtAuthentication(IdentityUser user);
    }
}
