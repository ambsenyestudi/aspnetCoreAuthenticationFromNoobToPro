#Claims

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
    - [Configure you project](#Configure-you-project)
    - [Claims factory](#Claims-factory)
    - [User sigIn manager](#User-sigIn-manager)
    - [Custom claims](#Custom-claims)
    - [User store](#User-store)
    - [As a result] (#As-a-result)

## Preface

Claims define what user is it's a simple claim type claim value pair for more info [Claim-based authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-3.1)
> For teaching purpuses on this section:
> -We only create useless claims (through a ClaimsFactory)
> -Add Athorize Atributte to the Privacy() metod on (HomeController)(/src/Controllers/HomeController.cs)
> -Assign claims to the HttpContext

## Work done

### Configure you project

At [Startup.cs](/src/Ambseny.WebAplication/Startup.cs) we configure our ClaimsPrincipalFactory at ConfigureServices method:
```
 services.AddIdentity<EasyUser, IdentityRole>()
    .AddUserStore<EasyUserStore>()
    .AddRoleStore<EasyRoleStore>()
    .AddSignInManager<EasyUserSignInManager>()
    .AddClaimsPrincipalFactory<EasyUserClaimsPrincipalFactory>();
```

### Claims factory

We need to define our own claims factory to manage EasyUser types so, [EasyUserClaimsPrincipalFactory](/src/Ambseny.WebAplication/Data/User/EasyUserClaimsPrincipalFactory.cs)
creates the identity of our user adding [custom claims](#Custom-claims).

At this point it only creates a silly claim (sid is simple the user identifier claim type), just o feell the flow.

### User sigIn manager 

Since we configured our Claims factory at Startup, now we have access to such dependency at [EasyUserSignInManager](/src/Ambseny.WebAplication/Data/User/EasyUserSignInManager.cs) level. 

Now we can user the Context property to setup the Identity, composed of custom claims.

> Actually the property User of the HttpContext has a misleading name since, Context User contains no information about our User only Claims, think of it as an access card with no name on it.

### Custom claims

Now we need another data base model to store our custom claims [UserClaim](/src/Ambseny.WebAplication/Model/Users/UserClaim.cs).
It's a simple claim type/value related to the user by id. To store it we just created a new UserClaims DbSet at [EasyUserDbContext](/src/Ambseny.WebAplication/Data/EasyUserDbContext.cs) and we are good to go.

### User store

Now to ilustratre this silly example, every time we regisister a user, at [UserStore](/src/Ambseny.WebAplication/Data/User/EasyUserStore.cs) level, custom claim. 
Our custom claims is a UserClaim that olds the a Sid Claim tat is stored at our dbContex, so when signing in we can get when creating our identity

### As a result

Now for the first time we have an Autorized Privacy page, so we can se the configuration of the cookie we made a couple of secction ago at work. 
```
services.ConfigureApplicationCookie(config => {
    config.Cookie.Name = "IdentityAutheticate.Cookie";
    config.LoginPath = "/Account/Login";
});
```
If you try to access via Url to privacy /Home/Privacy your will be redirected to our Login page 

Next we will manage some user using policy access


