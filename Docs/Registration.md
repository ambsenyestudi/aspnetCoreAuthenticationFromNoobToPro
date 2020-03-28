# Register new user
Now it's time to be able to register new users.

### Register View
Create a Register method on your [account controller](/src/Ambseny.WebAplication/Controllers/AccountController.cs).

Then scaffold a view with create template for Easy user just as we did for the login.

We need a create user model to have a field confirm password

And we'll change all password input of our view to type password

We need at user store de dbcontext in order to have persist our newly created users