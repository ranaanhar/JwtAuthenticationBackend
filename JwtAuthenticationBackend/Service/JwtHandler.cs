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
        public JwtHandler(IConfiguration configuration, ILogger<JwtHandler> logger, Database database)
        {
            _configuration = configuration;
            _logger = logger;
            _database = database;
        }

        /// <summary>
        /// return token
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ResponseAuthentication</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResponseAuthentication? getJwtAuthentication(ApplicationUser user)
        {
            try
            {
                DateTime expire = DateTime.UtcNow.AddHours(Data.DataConstants.TokenExpirationTime);

                if (user == null)
                {
                    //TODO Fix this
                    throw new ArgumentNullException(nameof(user));
                }
                var handler = new JwtSecurityTokenHandler();
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:issuer"],
                    audience: _configuration["JWT:audience"],
                    claims: setClaims(user),
                    notBefore: DateTime.UtcNow,
                    expires: expire,
                    signingCredentials: getSignInCredentials()
                    );

                return new ResponseAuthentication()
                {
                    Expiration = getExpires(expire),
                    Token = handler.WriteToken(token),
                    RefreshToken=GenerateRefreshToken() 
                };
            }
            catch (Exception exp)
            {
                _logger.LogInformation(exp, exp.Message);
            }
            return null;
        }

        /// <summary>
        /// Generate Refresh Token 
        /// </summary>
        /// <returns>string</returns>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        /// <summary>
        /// Set Token Security Key
        /// </summary>
        /// <returns></returns>
        private SigningCredentials getSignInCredentials()
        {
            var key = _configuration["JWT:key"]!;
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }


        /// <summary>
        /// Create Claims Based of Given User
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>Claim[]</returns>
        private Claim[] setClaims(ApplicationUser user)
        {
            var role = getRole(user);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims.ToArray();
        }


        /// <summary>
        /// Validate Token And Return Principals From Token
        /// </summary>
        /// <param name="token"><string/param>
        /// <returns>ClaimsPrincipal</returns>
        /// <exception cref="SecurityTokenException"></exception>
        public ClaimsPrincipal? GetClaimsPrincipal(string? token){
            if(string.IsNullOrEmpty(token))
                return null;

            var validationParameters=new TokenValidationParameters{
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:issuer"],
                ValidAudience = _configuration["JWT:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!))
            };
            var jwtHandler=new JwtSecurityTokenHandler();
            var Principal=jwtHandler.ValidateToken(token,validationParameters,out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token.");
            }
            return  Principal;
        } 


        /// <summary>
        /// Retrieve Role Of User From DataBase 
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>string</returns>
        private string? getRole(ApplicationUser user)
        {
            var RoleId = _database.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).First();
            if (!string.IsNullOrEmpty(RoleId))
            {

                var role = _database.Roles.Where(x => x.Id == RoleId).Select(x => x.Name).First();
                return role;
            }
            return null;
        }

        /// <summary>
        /// Convert Expiration Time To Number Format
        /// </summary>
        /// <param name="time">DateTime</param>
        /// <returns>string</returns>
        private static string getExpires(DateTime time)
        {
            var unixEpoch = DateTime.UnixEpoch;
            var timeFrom1970 = time - unixEpoch;
            var timeToMilliseconds = Math.Ceiling(timeFrom1970.TotalMilliseconds);
            return timeToMilliseconds.ToString();
        }
    }

    public interface IJwtHandler
    {
        public ResponseAuthentication? getJwtAuthentication(ApplicationUser user);
        public ClaimsPrincipal? GetClaimsPrincipal(string? token);
    }
}
