using System;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthenticationBackend.Data;

public class DatabaseInitializer
{
    private readonly ModelBuilder modelBuilder;
    public DatabaseInitializer(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    //seeding database
    public void Seed()
    {
        //create admin and user roles
        var admin_role = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = DataCostants.Admin,
            NormalizedName = DataCostants.Admin_Normalize
        };
        var user_role = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = DataCostants.User,
            NormalizedName = DataCostants.User_Normalize
        };



        //initialize roles
        this.modelBuilder.Entity<IdentityRole>().HasData(admin_role, user_role);



        //create admin user
        var admin_user = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@provider.com",
            EmailConfirmed = true,
            NormalizedEmail = "ADMIN@PROVIDER.COM",
            UserName = Data.DataCostants.Admin,
            NormalizedUserName = Data.DataCostants.Admin_Normalize,
            PhoneNumber = "9111111111",
            PhoneNumberConfirmed = true,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        //set password for admin user
        PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
        admin_user.PasswordHash = hasher.HashPassword(admin_user, "SupperSecretPassword4Admin");

        //initialize admin user
        this.modelBuilder.Entity<IdentityUser>().HasData(admin_user);

        //add admin user to roles
        this.modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = admin_role.Id,
            UserId=admin_user.Id,
        });
    }
}
