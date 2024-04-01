using Microsoft.AspNetCore.Identity;

namespace JwtAuthenticationBackend.Model
{
    public class ResponseSignup
    {
        public ResponseSignup(IdentityUser user) {
            if (user == null) {
                return;
            }
            Username = user.UserName;
        }
        public string? Username { get; }=null;

        public override string ToString()
        {
            return Username ?? string.Empty;
        }
    }
}
