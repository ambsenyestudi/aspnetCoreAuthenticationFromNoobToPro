# Setup Identity on ASP.NET	Core 

## Nugets

In order to have your asp net core identity ready to follow up these projects you need to install the following nugets:

- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.InMemory

## Implementing your User type

In most article you'll see that they recommend you to use an Entity Framework Core context that inherits from [IdentityDbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.identitydbcontext?view=aspnetcore-3.0). 
That's fine if you want to use the **default class for users** called [ApplicationUser](https://docs.microsoft.com/en-us/aspnet/core/migration/identity?view=aspnetcore-3.1).

> Since we want to keep it simple and build up from there, we are going to implement our own type of users so no need for anything but a regular Entity Framework Core DbContext.

As mentioned to use your own type of user you must write classes that implement the following interfaces:

- IUserStore<TUser> 
- IRoleStore<TUser>

> Since we are implementing stores we could very well use other data persistence systems apart from Entity Framework Core.
