using JwtAuthenticationBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var configuration = builder.Configuration;

service.AddSwaggerGen();
service.AddControllers();

service.AddDbContextPool<Database>(optionsAction => {
    var connectionString = configuration.GetConnectionString("default");
    optionsAction.UseMySQL(connectionString!); });


var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.MapControllers();
app.Run();
