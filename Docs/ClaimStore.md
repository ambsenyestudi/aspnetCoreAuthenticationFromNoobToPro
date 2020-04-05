# Claims store

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
    - [UserManager](#UserManager)
    - [UserStore](#UserStore)
    - [UserServices](#UserServices)
    - [IdentityErrorDescriber](#IdentityErrorDescriber)
    - [Setup](#Setup)

## Preface

As seen on the previous chapter UserStore Implments many interfaces being one of them IUserClaimStore<TUser>, so let's take advantage of it to refactore our code through User manager

## Work done

### UserManager

We erased all EF code from [EasyUserClaimsPrincipalFactory](/src/Ambseny.WebAplication/Data/User/EasyUserClaimsPrincipalFactory.cs) and made use of 
[EasyUserManager](/src/Ambseny.WebAplication/Data/User/EasyUserManager.cs), 
specifically the methods provided by UserManager, (we just overrided them to show which ones).
```
public override Task<IdentityResult> AddClaimAsync(EasyUser user, Claim claim)
{
    return base.AddClaimAsync(user, claim);
}
public override Task<IdentityResult> AddClaimsAsync(EasyUser user, IEnumerable<Claim> claims)
{
    return base.AddClaimsAsync(user, claims);
}
public override Task<IList<Claim>> GetClaimsAsync(EasyUser user)
{
    return base.GetClaimsAsync(user);
}
```

### UserStore

Now it implements IUserClaimStore<EasyUser> so [EasyUserStore](/src/Ambseny.WebAplication/Data/User/EasyUserStore.cs) exposes CRUD operations on the claims to the UserManager. Rendering the late ClaimService useless.

### UserServices
Now [UsersService](/src/Ambseny.WebAplication/Services/Users/UsersService.cs) has as an injected dependecy EasyUserManager insted of IClaimsServices.
Basic refactor that now is more ASP Net Core compliant.