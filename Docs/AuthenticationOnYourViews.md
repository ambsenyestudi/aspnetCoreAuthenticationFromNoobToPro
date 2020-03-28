# Show or hide parts of your view depending on Authentication

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
    - [Models and Stores](#Models-and-Stores)
    - [Views](#Views)
    - [Setup and configuration](#Setup-and-configuration)

## Preface

We inspire from this [Introduction to Identity on ASP.NET core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) 
article for our very first steps, concretely the [Log out section](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio#log-out), 
the above section implements a new shared view Pages/Shared/_LoginPartial.cshtml in order to have some parts visible or hidden depending on users sign-in state.

## Work done

### Models and Stores

As mentioned in [Setup Identity on ASP.NET Core](Docs/SetupAspNetIdentity.md) we implemented the following:

- Models
	- EasyUser (at the Users folder)
- Everything related to EF Core is at Data
	- EasyUserDbContext
	- Stores for Identity at (Data/User folder)
		- EasyUserStore implements IUserStore
		- EasyRoleStore implements IRoleStore

### Views

Instead of writing a new *partial view* we modified [Views/Shared/_Layout.cshtml](/blob/00_EasyUser/src/Ambseny.WebAplication/Views/Shared/_Layout.cshtml) 
using the following attribute to hide/show parts of the view.
```
@inject SignInManager<EasyUser> SignInManager
```

### Setup and configuration
To have access to this module we must add Identity and dbContext using the very convenient extension methods ready for it  at Startup.cs at ConfigureServices method
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddDbContext<EasyUserDbContext>(options =>
        options
        .UseInMemoryDatabase("Memory")
        //.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );
    services.AddIdentity<EasyUser, IdentityRole>()
        .AddUserStore<EasyUserStore>()
        .AddRoleStore<EasyRoleStore>();
}
```

Once you are done, you get parts of you layout hidden and nothing more, simple right?
