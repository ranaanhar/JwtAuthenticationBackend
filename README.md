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

Make sure you have installed dotnet ef on your system.
visite [Microsoft web site](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) for installing dotnet ef.

After deploying the app we have no database and migrations on the server side for that we need to add migration in code:  

```c#
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Database>();
    context.Database.Migrate();
}
```



<h2>Deploying</h2>

For deploying the app on linux based system we can use the Nginx web serve. 
for more information see [Microsoft website site](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-8.0&tabs=linux-ubuntu).

We can publish the app in Visual Studio or from the terminal then for transfer project files we can use 'scp' command in terminal:

```
scp projectfile doman@ipofserver: /var / www
```

For configure Nginx we must go to /etc / nginx / nginx.conf and edit like below:

```
server {
    listen 80 default_server;
    listen [::]:80 default_server;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2 default_server;
    listen [::]:443 ssl http2 default_server;

    ssl_certificate /etc/nginx/cert.pem;
    ssl_certificate_key /etc/nginx/cert.key;

    location / {
        proxy_pass http://dotnet;
        proxy_set_header Host $host;
    }
}

upstream dotnet {
    zone dotnet 64k;
    server 127.0.0.1:5000;
}
```

for more information see [Nginx website site](https://www.nginx.com/blog/tutorial-proxy-net-core-kestrel-nginx-plus/). 



