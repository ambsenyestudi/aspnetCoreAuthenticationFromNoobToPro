# Register new user
Now it's time to be able to register new users.

### Register View
Create a Register method on your [account controller](/src/Ambseny.WebAplication/Controllers/AccountController.cs).

Then scaffold a view with create template for Easy user just as we did for the login.

We need a create user model to have a field confirm password

And we'll change all password input of our view to type password

And we added a extra validation on User name so it displays login errors and user creation error

### User store

We need at [EasyUserStore](/src/Ambseny.WebAplication/Data/User/EasyUserStore.cs) de dbcontext in order to have persist our newly created users and implement FindByIdAsync and FindByNameAsync 

### Signin manager

No overrides sign with password 

### User manager 

Now create users using user store

### CreateEasyUser Model

Add a new [model](/src/Ambseny.WebAplication/Models/Users/CreateEasyUser.cs), we need a confirm password validation for our Register view

