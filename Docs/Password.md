#Password

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
-   [Seeding](#Seeding)
-   [Policy setup](#Policy-setup)
-   [Claims update](#Claims-update)
-   [SignIn manager errata](#SignIn-manager-errata)
-   [Profile](#Profile)
-	[Solidify our code](#Solidify-our-code)
-   [Manage claims](#Manage-claims)

##Preface

Now that we have some easy undestanding of what is happening with claims policies and so on, it's time that we make better use of Identity. Let's explore and further use it's implementation.

###Work done

We completely changed the signinmanager in order to use its true implementation with the context signin. At the preceding section, to signin we used our implementation of 
signing that called the base classe's method to sign in and then we called our method UpdateContextAsync to set the Identity at context user.

No we are going take a look at the base implementation of [SignInManager](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/SignInManager.cs) to improve our's.

So at about line 204 at method SignInAsync we can observe this:
```
public virtual Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
{
    var additionalClaims = new List<Claim>();
    if (authenticationMethod != null)
    {
        additionalClaims.Add(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
    }
    return SignInWithClaimsAsync(user, authenticationProperties, additionalClaims);
}
```
Then at about line 231 you can find the SignInWithClaimsAsync method
```
public virtual async Task SignInWithClaimsAsync(TUser user, AuthenticationProperties authenticationProperties, IEnumerable<Claim> additionalClaims)
{
    var userPrincipal = await CreateUserPrincipalAsync(user);
    foreach (var claim in additionalClaims)
    {
        userPrincipal.Identities.First().AddClaim(claim);
    }
    await Context.SignInAsync(IdentityConstants.ApplicationScheme,
        userPrincipal,
        authenticationProperties ?? new AuthenticationProperties());
}
```

So inted of setting the context User we user tis sing in methods. Of course we need to user the scheme name so there wount be any sing in at all. Luckily [IdentityConstants](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/IdentityConstants.cs)
privides us with the applicationscheme of its own setting at [IdentityServiceCollectionExtensions](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/IdentityServiceCollectionExtensions.cs)

So efectivelly all is set for you at AddIndentity() and its not configurable for you any more, even the rediret path to login!!!