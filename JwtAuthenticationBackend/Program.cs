using JwtAuthenticationBackend.Authorization;
using JwtAuthenticationBackend.Data;
using JwtAuthenticationBackend.Model;
using JwtAuthenticationBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var configuration = builder.Configuration;
var cors = "http://localhost:4200";

builder.Logging.AddConsole();

service.AddSwaggerGen();
service.AddControllers();



//Add Database Connector
service.AddDbContextPool<Database>(optionsAction =>
{
    var connectionString = configuration.GetConnectionString("PostgresSQL");

    //Use postgresql 
    optionsAction.UseNpgsql(connectionString!);
});



//Add role and user from identityCore
service.AddIdentityCore<ApplicationUser>(setupAction =>
{
    setupAction.SignIn.RequireConfirmedAccount = true;
    setupAction.SignIn.RequireConfirmedPhoneNumber = false;
    setupAction.Password.RequireDigit = false;
    setupAction.Password.RequiredLength = 8;
    setupAction.Password.RequireNonAlphanumeric = false;
    setupAction.Password.RequireLowercase = false;
    setupAction.Password.RequireUppercase = false;
    setupAction.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<Database>();



//Add Jwt Handler to Scope
service.AddScoped<IJwtHandler, JwtHandler>();


service.AddSingleton<IAuthorizationMiddlewareResultHandler,CustomAuthorizationMiddlewareHandler>();


service.AddSingleton<IAuthorizationHandler,CustomAuthorizationHandler>();

//Add Authentication
service.AddAuthentication(optionsAction =>
{
    optionsAction.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).
AddJwtBearer(configureOptions =>
{
    configureOptions.SaveToken = true;
    configureOptions.IncludeErrorDetails = true;
    configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime=true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JWT:issuer"],
        ValidAudience = configuration["JWT:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]!))
    };
});



//Add Authorization
service.AddAuthorization(config =>
{
    config.AddPolicy("userPolicy", policy =>
    {
        policy.RequireRole("User").
        RequireAuthenticatedUser(); 
    });
});



//Add Cors
service.AddCors(config =>
{
    config.AddPolicy("cors", policy =>
    {
        policy.WithOrigins(cors).AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

//Use Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
