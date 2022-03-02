# Endpoint
[![NuGet](https://buildstats.info/nuget/Allagi.Endpoint.Cli?includePreReleases=true)](http://www.nuget.org/packages/Allagi.Endpoint.Cli "Download Allagi.Endpoint.Cli from NuGet")

ASP.NET Core API Code Generator via a simple command line interface with first class support for generating Minimal Api's.

Endpoint builds the solution file, project file, models, dtos, API's and bicep deployment templates without you having to write a single line of code. You can also add more models, dtos, etc...as the project grows.

## Build an API in 60 seconds Demo

[![Build Asp.Net Core Minimal Api with CRUD functionality in 60 seconds from scratch](https://img.youtube.com/vi/whNVtybidcI/0.jpg)](https://www.youtube.com/watch?v=whNVtybidcI)

## Features

This tool will create .NET Solution with the following feature(s):

* [Strongly Typed Ids](https://github.com/andrewlock/StronglyTypedId): Aggregates are generated with Strongly Typed Id's by default
* [DDD](https://github.com/QuinntyneBrown/Endpoint): Option for Clean Architecture inspired template
* [Minimal Api](https://github.com/QuinntyneBrown/Endpoint): Option for minimal api
* [Serilog](https://github.com/QuinntyneBrown/Endpoint): Logging with Serilog built in
* [FluentValidation](https://github.com/QuinntyneBrown/Endpoint): Boilerplate for server side validation built in
* [MediatR](https://github.com/QuinntyneBrown/Endpoint): CQRS
* [Entity Framework Core](https://github.com/QuinntyneBrown/Endpoint): Best practices built in
* [Unit Tests](https://github.com/QuinntyneBrown/Endpoint): Generated Unit Tests
* [Bicep](https://github.com/Azure/bicep): Auto generated Bicep templates and deployment scripts. ref: [docs.microsoft.com](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep)


## Installation

Install the [Endpoint NuGet Package](https://www.nuget.org/packages/Allagi.Endpoint.Cli).


### .NET Core CLI

```
dotnet tool install --global Allagi.Endpoint.Cli
```
## SYNOPSIS

```
endpoint [-n] [-r] [--properties] [-a or --minimalapi]
```

## EXAMPLES

```
endpoint -n TodoApp -r Todo --properties Title:string,IsComplete:bool --minimal-api true
```

Generates a CRUD Minimal api for a Todo entity and Unit Test Project
