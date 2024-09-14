using JwtAuthenticationBackend.Data;
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
    public class JwtHandler : IJwtHandler
    {
        ILogger<JwtHandler> _logger;
        IConfiguration _configuration;
        Database _database;
        const int ExpirationTime = 10;
        public JwtHandler(IConfiguration configuration, ILogger<JwtHandler> logger, Database database)
        {
            _configuration = configuration;
            _logger = logger;
            _database = database;
        }

        public ResponseAuthentication? getJwtAuthentication(IdentityUser user)
        {
            try
            {
                DateTime expire = DateTime.UtcNow.AddMinutes(ExpirationTime);
                if (user == null)
                {
                    throw new ArgumentNullException("user null received!");
                }
                var handler = new JwtSecurityTokenHandler();
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:issuer"],
                    audience: _configuration["JWT:audience"],
                    claims: getClaims(user),
                    notBefore: DateTime.UtcNow,
                    expires: expire,
                    signingCredentials: getSigninCredentials()
                    );
                getExpires(expire);
                return new ResponseAuthentication() { Expiration = getExpires(expire), Token = handler.WriteToken(token) };
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
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        }

        private Claim[] getClaims(IdentityUser user)
        {
            var role = getRole(user);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims.ToArray();
        }

        private string? getRole(IdentityUser user)
        {
            var RoleId = _database.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).First();
            _logger.LogInformation($"RoleId = {RoleId},");
            if (!string.IsNullOrEmpty(RoleId))
            {

                var role = _database.Roles.Where(x => x.Id == RoleId).Select(x => x.Name).First();
                _logger.LogInformation($"RoleName = {role} Added.");
                return role;
            }
            return null;
        }

        private string getExpires(DateTime time)
        {

            var timefrom1970 = time - new DateTime(1970, 1, 1);
            var timeToMilliseconds = Math.Ceiling(timefrom1970.TotalMilliseconds);
            _logger.LogInformation($"Expires: {timeToMilliseconds}");
            return timeToMilliseconds.ToString();
        }
    }

    public interface IJwtHandler
    {
        public ResponseAuthentication? getJwtAuthentication(IdentityUser user);
    }
}
