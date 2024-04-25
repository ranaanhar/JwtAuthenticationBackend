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

```
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
visite <a>asp.net</a> for installing dotnet ef.



