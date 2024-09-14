using JwtAuthenticationBackend.Data;
using JwtAuthenticationBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var configuration = builder.Configuration;
var cors = "http://localhost:4200";

builder.Logging.AddConsole();
//builder.Logging.ClearProviders();

service.AddSwaggerGen();
service.AddControllers();

service.AddDbContextPool<Database>(optionsAction => {
    var connectionString = configuration.GetConnectionString("psql");
    //Use mysql
    //optionsAction.UseMySQL(connectionString!);

    //Use postgresql 
    optionsAction.UseNpgsql(connectionString!);
    });



//Add role and user from identityCore
service.AddIdentityCore<IdentityUser>(setupAction =>
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

service.AddScoped<IJwtHandler,JwtHandler>();

service.AddAuthentication(optionsAction => {
    optionsAction.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).
AddJwtBearer(configureOptions => {
    configureOptions.SaveToken = true;
    configureOptions.IncludeErrorDetails = true;
    configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey=true,
        ValidIssuer = configuration["JWT:issuer"],
        ValidAudience = configuration["JWT:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]!))
    };
});
service.AddAuthorization(config => {
    config.AddPolicy("userPolicy",policy=>policy.RequireRole("User"));
 });
service.AddCors(config =>
{
    config.AddPolicy("cors", policy =>
    {
        policy.WithOrigins(cors).AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
