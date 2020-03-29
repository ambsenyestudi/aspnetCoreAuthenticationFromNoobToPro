# Multy Policies

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
    - [Seeding](#Seeding)
    - [Policy setup](#Policy-setup)
    - [Claims update](#Claims-update)
    - [Profile](#Profile)
    - [Manage claims](#Manage-claims)
## Preface

As mentioned before, we now need at two more policies and seed to more users.

## Work done

### Seeding

To seed our reviewer named Bob, we just modified [Program.cs](/src/Ambseny.WebAplication/Program.cs) to add to the Seed method Alice and Admin.
Alice is an editor an Admin is an administrator

### Policy setup

We added to our Authorization configuration at [Startup.cs](/src/Ambseny.WebAplication/Startup.cs) two more policies and modifies our UserReviewPoliciy
```
services.AddAuthorization(options =>
{
    options.AddPolicy("Minimal", policy => policy.RequireClaim(ClaimTypes.Sid));
    options.AddPolicy("UserReviewer", policy => 
        policy.RequireClaim(AmbsenyClaimTypes.ManageUsers.ToString(), 
            AmbsenyManageUserClaims.Review.ToString(),
            AmbsenyManageUserClaims.Edit.ToString(),
            AmbsenyManageUserClaims.Administrate.ToString()
        )
    );
    options.AddPolicy("UserEditor", policy => 
        policy.RequireClaim(AmbsenyClaimTypes.ManageUsers.ToString(),
            AmbsenyManageUserClaims.Edit.ToString(),
            AmbsenyManageUserClaims.Administrate.ToString()
        )
    );
    options.AddPolicy("UserAdministrator", policy => policy.RequireClaim(AmbsenyClaimTypes.ManageUsers.ToString(), AmbsenyManageUserClaims.Administrate.ToString()));
});
```
Eash lower policy includes de claims of the higher ones.

### Claims update

We update our claim at [UserClaim.cs](/src/Ambseny.WebAplication/Models/Users/UserClaim.cs) to fit the new two privileges:
- Edit
- Administrate

### Profile

Now from profile if your fit te following policies:
> UserEditor policy you will be able to Edit the profile 
> UserReviewer policy get back to list 

### Manage claims

Now we have two more scenarios when managing claims at [ManageController.cs](/src/Ambseny.WebAplication/Controllers/ManageController.cs):
- Edit a profile
- Delete a profile
> So now our controller implements 3 different policy authorizations so:
> - Reviewers can see the list and go to every profile detail
> - Editors can also Edit the profile privileges 
> - Administrators can also Delete profiles

