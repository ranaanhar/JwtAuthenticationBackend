using JwtAuthenticationBackend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var configuration = builder.Configuration;

service.AddSwaggerGen();
service.AddControllers();

service.AddDbContextPool<Database>(optionsAction => {
    var connectionString = configuration.GetConnectionString("default");
    optionsAction.UseMySQL(connectionString!); });

service.AddIdentityCore<IdentityUser>(setupAction => {
    setupAction.SignIn.RequireConfirmedAccount = true; 
    setupAction.SignIn.RequireConfirmedPhoneNumber = false;
    setupAction.Password.RequireDigit = false;
    setupAction.Password.RequiredLength = 8;
    setupAction.Password.RequireNonAlphanumeric = false;
    setupAction.Password.RequireLowercase = false;
    setupAction.Password.RequireUppercase = false;
    setupAction.User.RequireUniqueEmail = true;
}); 

service.AddAuthorization(config => { });

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthorization();
app.MapControllers();
app.Run();
