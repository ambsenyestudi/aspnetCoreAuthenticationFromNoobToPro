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

### UserManager

We implemented the check password a user manager taking example in [UserManager](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Extensions.Core/src/UserManager.cs) 
at Identity/Extensions.Core where at line about 700 you can observe the following method
```
public virtual async Task<bool> CheckPasswordAsync(TUser user, string password)
{
    ThrowIfDisposed();
    var passwordStore = GetPasswordStore();
    if (user == null)
    {
        return false;
    }

    var result = await VerifyPasswordAsync(passwordStore, user, password);
    if (result == PasswordVerificationResult.SuccessRehashNeeded)
    {
        await UpdatePasswordHash(passwordStore, user, password, validatePassword: false);
        await UpdateUserAsync(user);
    }

    var success = result != PasswordVerificationResult.Failed;
    if (!success)
    {
        Logger.LogWarning(0, "Invalid password for user {userId}.", await GetUserIdAsync(user));
    }
    return success;
}
```
Then at about line 842
```
protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<TUser> store, TUser user, string password)
{

    var hash = await store.GetPasswordHashAsync(user, CancellationToken);
    if (hash == null)
    {
        return PasswordVerificationResult.Failed;
    }
    return PasswordHasher.VerifyHashedPassword(user, hash, password);
}
```
The store of the preceding method is simpley casting the user store to userpassword store (at about line 2591)
```
private IUserPasswordStore<TUser> GetPasswordStore()
{
    var cast = Store as IUserPasswordStore<TUser>;

    if (cast == null)
    {
        throw new NotSupportedException(Resources.StoreNotIUserPasswordStore);
    }

    return cast;
}
```
### User store
We added the normalized name to make it easyer to find use but if we observ the source code we get form the baseclass [UserStoreBase](https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Extensions.Stores/src/UserStoreBase.cs)
at Identity/Extensions.Stores it implements all the interfaces
```
public abstract class UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken> :
        IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserAuthenticationTokenStore<TUser>,
        IUserAuthenticatorKeyStore<TUser>,
        IUserTwoFactorRecoveryCodeStore<TUser>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
```
An thats the casting mentioned at the previous section happens.
As seen here now our Password becomes PasswordHash at EasyUser in order to implemetn IUserPasswrodStore for EasyUserStore.
### IdentityErrorDescriber
Just to avoid litering everythin with codes an everry thing, we created a [AmbsenyIdentityErrorDescriber](/src/Ambseny.WebAplication/Data/AmbsenyIdentityErrorDescriber.cs) class to add on the exiting ones