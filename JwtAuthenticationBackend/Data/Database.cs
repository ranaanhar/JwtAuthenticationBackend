using JwtAuthenticationBackend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace JwtAuthenticationBackend.Data
{
    public class Database:IdentityDbContext<ApplicationUser>
    {

        public Database(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new DatabaseInitializer(builder).Seed();
        }
    }
}
