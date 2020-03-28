# Sign In

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
    - [Configure you project](#Configure-you-project)
    - [Create an account controller](#Create-an-account-controller)
    - [User manager](#User-manager)
    - [User sigIn manager](#User-sigIn-manager)
    - [Layout](#Layout)

    
## Preface

At this point we want to see some result in our UI of a user login in, let's make a fake login. That means any user will be able to log in just typing name and password.

In order to make some kind of validation we are going to mark name an password of easy user as required:
```
[Required]
public string Name { get; set; }
[Required]
public string Password { get; set; }
```
So at least we will have to type something to log in.

We will add some tell at layout that we are logged in and we will add cookies, so the browser holds our Identity

## Work done

### Configure you project

At [Startup.cs](/src/Ambseny.WebAplication/Startup.cs) we need to configure two services explained later on this document as transients and add the cookie authentication at ConfigureServices
```
services.AddTransient<EasyUserSignInManager>();
services.AddTransient<EasyUserManager>();
services.AddIdentity<EasyUser, IdentityRole>()
    .AddUserStore<EasyUserStore>()
    .AddRoleStore<EasyRoleStore>()
    .AddSignInManager<EasyUserSignInManager>();

services.ConfigureApplicationCookie(config => {
    config.Cookie.Name = "IdentityAutheticate.Cookie";
    config.LoginPath = "/Account/Login";
});

services.AddAuthentication();
```
We also need to add UseAuthentication just before UseAuthorization at Configure
```
app.UseAuthentication();
app.UseAuthorization();
```

### Create an account controller

We will create an account controller with a login method.
In order to keep our identity, we need two more custom classes to fi our user:
- [EasyUserManager](/src/Ambseny.WebAplication/Data/User/EasyUserManager.cs)
- [EasyUserSignInManager](/src/Ambseny.WebAplication/Data/User/EasyUserSignInManager.cs)

We need to inject EasyUserSignInManager dependency at controller the constructor
```
public AccountController(EasyUserSignInManager signInManager)
{
    this.signInManager = signInManager;
}
```
EasyUserSignInManager has many dependencies but UserManager is impending which is a UserManager
```
public EasyUserSignInManager(
    UserManager<EasyUser> userManager, 
    IHttpContextAccessor contextAccessor, 
    IUserClaimsPrincipalFactory<EasyUser> claimsFactory, 
    IOptions<IdentityOptions> optionsAccessor, 
    ILogger<SignInManager<EasyUser>> logger, 
    IAuthenticationSchemeProvider schemes, 
    IUserConfirmation<EasyUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
{
}
```
#### User manager

This is the class that Identity complains if we don't have. Right now, it's useless for us but we will use it to register users in the futures. It extends UserManager<TUser>

We implemented our own type EasyUserManager so we can manage our EasyYser

### User sigIn manager

This is the class we need in order to have sign in privileges on the cookie. It extends SignInManager<TUser>

We implemented our own type EasyUserSignInManager so we can sign in our EasyUser.

Identity needs to be able to recognize a user, so we need to implement the following methods at [EasyUserStore](/src/Ambseny.WebAplication/Data/User/EasyUserStore.cs):
```
public Task<string> GetUserIdAsync(EasyUser user, CancellationToken cancellationToken) => 
    Task.FromResult(Guid.NewGuid().ToString());
        
public Task<string> GetUserNameAsync(EasyUser user, CancellationToken cancellationToken) =>
    Task.FromResult(user.Name);
```
So every time he get a user id we will return a new Guid, and simply return the users name at get user name.

This is made this way, so any user is sign-in, form the security point of view is utterly useless, but it gives us some perspective on how to update UI for signed users.

### Layout

We updated our [Views/Shared/_Layout.cshtml](/src/Ambseny.WebAplication/Views/Shared/_Layout.cshtml) so now, our navigation bar has a link (looking like a button) 
if you are not logged in or displays User's name if we are logged in

> Remember to erase the Cookie from the browser if you want to "log out"
To erase the cookie, use the developer tools of your browser.

If edge, developers’ tools (F12), Storage tab, Cookies, and erase our named cookie (in our case IdentityAuthenticate.Cookie)
