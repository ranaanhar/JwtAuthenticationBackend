using JwtAuthenticationBackend.Data;
using JwtAuthenticationBackend.Model;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthenticationBackend.Service
{
    public class JwtHandler : IJwtHandler
    {
        readonly ILogger<JwtHandler> _logger;
        readonly IConfiguration _configuration;
        readonly Database _database;
        //token expiration time in hour
        const int ExpirationTime = 2;
        //refresh token expiration time in day
        const int RefreshExpirationTime=1;
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
                DateTime expire = DateTime.UtcNow.AddHours(ExpirationTime);
                if (user == null)
                {
                    //TODO Fix this
                    throw new ArgumentNullException(nameof(user));
                }
                var handler = new JwtSecurityTokenHandler();
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:issuer"],
                    audience: _configuration["JWT:audience"],
                    claims: getClaims(user),
                    notBefore: DateTime.UtcNow,
                    expires: expire,
                    signingCredentials: getSignInCredentials()
                    );
                getExpires(expire);
                
                var refreshTokenExpiration=DateTime.UtcNow.AddHours(RefreshExpirationTime);
                var RefreshToken=new RefreshToken{
                    Token=GenerateRefreshToken(),
                    Expiration=getExpires(refreshTokenExpiration),
                    UserId=user.UserName,
                };
                //TODO : save refresh token in user 


                return new ResponseAuthentication() 
                { 
                    Expiration = getExpires(expire),
                     Token = handler.WriteToken(token),
                     RefreshToken=RefreshToken };
            }
            catch (Exception exp)
            {
                _logger.LogInformation(exp,exp.Message);
            }
            return null;
        }

        private static string GenerateRefreshToken(){
            var randomNumber=new byte[64];
            using var randomNumberGenerator=RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private SigningCredentials getSignInCredentials()
        {
            var key = _configuration["JWT:key"]!;
            var SecurityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        private Claim[] getClaims(IdentityUser user)
        {
            var role = getRole(user);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims.ToArray();
        }

        private string? getRole(IdentityUser user)
        {
            var RoleId = _database.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).First();
            if (!string.IsNullOrEmpty(RoleId))
            {

                var role = _database.Roles.Where(x => x.Id == RoleId).Select(x => x.Name).First();
                return role;
            }
            return null;
        }

        private static string getExpires(DateTime time)
        {
            var unixEpoch=DateTime.UnixEpoch;
            var timeFrom1970 = time - unixEpoch;
            var timeToMilliseconds = Math.Ceiling(timeFrom1970.TotalMilliseconds);
            return timeToMilliseconds.ToString();
        }
    }

    public interface IJwtHandler
    {
        public ResponseAuthentication? getJwtAuthentication(IdentityUser user);
    }
}
