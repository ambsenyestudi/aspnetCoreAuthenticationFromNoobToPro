#Password

## Table of content

- [Preface](#Preface)
- [Work done](#Work-done)
    - [SignInManager](#SignInManager)
    - [UserManager](#UserManager)
    - [UserStore](#UserStore)
    - [Seeding](#Seeding)
    - [IdentityErrorDescriber](#IdentityErrorDescriber)
    - [Setup](#Setup)

## Preface

As seen on the previous chapter UserStore Implments many interfaces being one of them IUserClaimStore<TUser>, so let's take advantage of it to refactore our code through User manager

### Work done