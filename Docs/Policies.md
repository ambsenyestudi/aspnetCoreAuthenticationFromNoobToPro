# Policies

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

## Preface

As mentioned before, we need at list one type of user with review privileges, given that we use in memory database for now, we need to seed said user every time the application starts.

## Work done

### Seeding

To seed our reviewer named Bob, we just modified [Program.cs](/src/Ambseny.WebAplication/Program.cs) to add a Seed method that adds the default sid claim and adds a new one. 
The new claim states that bob is a reviewer.

### Policy setup

We added to our Authorization configuration at [Startup.cs](/src/Ambseny.WebAplication/Startup.cs) configure two policies:
- Minimal it's de default one that only asks for a sid claim
- UserReviewer that for now only asks for the Review

### Claims update

In order to avoid the constants in the code base smell I added two enums at [UserClaim.cs](/src/Ambseny.WebAplication/Models/Users/UserClaim.cs):
- AmbsenyClaimTypes: to identify our very own claim types
- AmbsenyManageUserClaims: state our user claim value

### SignIn manager errata

Disclaimer from the las section we modified the [EasyUserSignInManager](/src/Ambseny.WebAplication/Models/Users/EasyUserSignInManager.cs) **PasswordSignInAsync** method to call 
SignInAsync after checking the password or else our cookie wasn't updated with the new signin state.

### Profile 

As stated in the requirements for this section, we need all user to be able to see their info on a profile view, so let’s create it.
> Using our default claim (Sid) we can get the user profile no problem.
But since we will use it this a detail page as well, we will leave the option of passing an id of the profile detail.

### Solidify our code

To be SOLID let's start at:
> - Single responsibility principle
>	- [Users service](/src/Ambseny.WebAplication/Services/Users/UsersService.cs) : covers all our needs related to users
>	- [Claims service](/src/Ambseny.WebAplication/Services/Claims/ClaimsService.cs) :  covers all our needs related to claims
Now all our controller having will only have to consume these dependencies

### Manage claims

Let's create a view that displays all users and their user manage status [ManageController.cs](/src/Ambseny.WebAplication/Controllers/ManageController.cs). This view
is only accessible to reviewers. 
```
[Authorize(Policy = "UserReviewer")]
public class ManageController : Controller
```
And from the view we can click on the profile link to watch profiles from all users
