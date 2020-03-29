# Policies

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
-   [Seeding](#Seeding)
-   [Claims update](#Claims-update)
-   [SignIn manager errata](#SignIn-manager-errata)
-   [Profile](#Profile)
-	[Solidify our code](# Solidify-our-code)
-   [Manage claims](#Manage-claims)
## Preface

As mentioned before we need at list one type of user with review privilges, given that we use in memory database for now, we need to seed said user every time the applicaiton starts.
## Work done

### Seeding

To seed our reviewer named Bob, we just modified [Program.cs](/src/Ambseny.WebAplication/Program.cs) to add a Seed method that adds the default sid claim and adds a new one. 
The new claim states that bob is a reviewer.

### Claims update

In order to avoid the constants in the code base smell I added two enums at [UserClaim.cs](/src/Ambseny.WebAplication/Models/Users/UserClaim.cs):
- AmbsenyClaimTypes: to identify our very own claim types
- AmbsenyManageUserClaims: state our user claim value

### SignIn manager errata

Disclaimer from the las section we modifies the [EasyUserSignInManager](/src/Ambseny.WebAplication/Models/Users/UserClaim.cs) **PasswordSignInAsync** method to call 
SignInAsync after checking the password or else our cookie wasn't updated with the new signin state.

### Profile 

As stated in the requierements for this section, we need all user to be able to see their info on a profile view, so lets create it.
> Using our default claim (Sid) we can get the user profile no problem.
But anyway as we will use it this a detail page as well se set the option of passing an id of the profile detail.

### Solidify our code

To be SOLID let's start at:
> - Single responsibility principle
>	- [Users service](/src/Ambseny.WebAplication/Services/Users/UsersService.cs) : covers all our needs related to users
>	- [Claims service](/src/Ambseny.WebAplication/Services/Claims/ClaimsService.cs) :  covers all our needs related to claims
Now all our controller having will only have to consume these dependencies

### Manage claims

Let's create a view that displays all users and their user manage status [ManageController.cs](/src/Ambseny.WebAplication/Controllers/ManageController.es). This view
is only accessible to reviewers. 
```
[Authorize(Policy = "UserReviewer")]
public class ManageController : Controller
```
And from the view we can click on the profile linke to watch profiles from all users