using System;
using System.Reflection.Metadata;
using JwtAuthenticationBackend.Model;
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
            Name = DataConstants.Admin_Role,
            NormalizedName = DataConstants.Admin_Normalize
        };
        var user_role = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = DataConstants.User_Role,
            NormalizedName = DataConstants.User_Normalize
        };



        //initialize roles
        this.modelBuilder.Entity<IdentityRole>().HasData(admin_role, user_role);



        //create admin user
        var admin_user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@provider.com",
            EmailConfirmed = true,
            NormalizedEmail = "ADMIN@PROVIDER.COM",
            UserName = Data.DataConstants.Admin,
            NormalizedUserName = Data.DataConstants.Admin_Normalize,
            PhoneNumber = "9111111111",
            PhoneNumberConfirmed = true,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        //set password for admin user
        var PasswordHash = new PasswordHasher<ApplicationUser>();
        admin_user.PasswordHash = PasswordHash.HashPassword(admin_user, "SupperSecretPassword4Admin");

        //initialize admin user
        this.modelBuilder.Entity<ApplicationUser>().HasData(admin_user);

        //add admin user to roles
        this.modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = admin_role.Id,
            UserId=admin_user.Id,
        });
    }
}
