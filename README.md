# ASP.NET Core authentication from noob to pro

## Table of content

- [Preface](#Preface)
- [Introduction](#Introduction)
- [Project](#Project)
    - [Authenticate your views](#Authenticate-your-views)
    - [Dummy SignIn](#Dummy-SignIn)

## Preface
This project is meant be a step by step form securing your web applications to using open-id for your suite of apps

## Introduction

First is first let's talk about Asp net core Identity

The very first step is [Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) to get you started. And you can get a fairly good sample at [Asp net core Identity samples for mvc](https://github.com/dotnet/aspnetcore/tree/master/src/Identity/samples/IdentitySample.Mvc)

## Project

These projects are intended to start very simple and add things progressively so anyone can follow along.

The different steps are saved in branches named using the following pattern oderNumber.nameOfSection so you don't get lost at any step.

In order to follow along these projects, you must set up Asp net core identity on you authenticated project.

To know more on hot to set it up read [Setup Identity](Docs/SetupAspNetIdentity.md) 

### Authenticate your views
Let's start simple. 

> On this first section we are only going to use de @inject attribute on a view to hide some sections.

First let's show [Authentication in our view](Docs/AuthenticationOnYourViews.md)

### Dummy SignIn

No just to feel the flow let's produe a dummy sign-in process.
[DummySing In](Docs/SignIn.md)

### Register Users

Let's add the functionality of registering users to our aplication
[Register users](Docs/Registration.md)
