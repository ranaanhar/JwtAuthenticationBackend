<h2>How To Run</h2>
For run use this command in the app directory: 

```
dotnet run .
```

After publishing use this command in a published directory:

```
dotnet JwtAuthenticationBackend.dll
```

<h3>Use MySql</h3>
In this project, I used MySQL for the database
keep in mind we use asp.net authentication classes in the Database class:

```c#
public class Database:IdentityDbContext<IdentityUser>
{
...
}
```

so before running the app, we must add migrations to the project, we can use dotnet cli :

```
dotnet ef migrations add initialidentity
```
and
```
dotnet ef database update
```

make sure you have installed dotnet ef on your system.
visite [Microsoft web site](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) for installing dotnet ef.

After deploying the app we have no database and migrations on the server side for that we need to add migration in code:  

```c#
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Database>();
    context.Database.Migrate();
}
```


