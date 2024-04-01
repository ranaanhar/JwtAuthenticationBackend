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
        ILogger<JwtHandler> logger;
        IConfiguration configuration;
        public JwtHandler(IConfiguration configuration, ILogger<JwtHandler> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public ResponseAuthentication? getJwtAuthentication(IdentityUser user)
        {
            try
            {
                if (user == null) { throw new ArgumentNullException("user"); }
                var handler = new JwtSecurityTokenHandler();
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:issuer"],
                    audience: configuration["JWT"],
                    claims:getClaims(user),
                    notBefore:DateTime.Now,
                    expires:DateTime.Now,
                    signingCredentials:getSigninCredentials()
                    );
                return new ResponseAuthentication() { Expiration = DateTime.Now.ToString() ,Token=handler.WriteToken(token)};
            }
            catch (Exception exp)
            {
                logger.LogInformation(string.Format("user null received! {0}",exp.Message));
            }
            return null;
        }

        private SigningCredentials getSigninCredentials()
        {
            var key = configuration["JWT:key"]!;
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
    }

    public interface IJwtHandler {
        public ResponseAuthentication? getJwtAuthentication(IdentityUser user);
    }
}
