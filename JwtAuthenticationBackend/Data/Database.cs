using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace JwtAuthenticationBackend.Data
{
    public class Database:IdentityDbContext<IdentityUser>
    {

        public Database(DbContextOptions options) : base(options)
        {
        }
    }
}
