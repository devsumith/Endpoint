# Endpoint
[![NuGet](https://buildstats.info/nuget/Allagi.Endpoint.Cli?includePreReleases=true)](http://www.nuget.org/packages/Allagi.Endpoint.Cli "Download Allagi.Endpoint.Cli from NuGet")


Model Driven Source Code Generator to build production grade Clean Architecture API's using ASP.NET Core.

## Features

This tool will create .NET Solution with the following feature(s):

* [Strongly Typed Ids](https://github.com/andrewlock/StronglyTypedId): Aggregates are generated with Strongly Typed Id's by default
* [DDD](https://github.com/QuinntyneBrown/Endpoint): Option for Clean Architecture inspired template
* [Minimal Api](https://github.com/QuinntyneBrown/Endpoint): Option for minimal api
* [Serilog](https://github.com/QuinntyneBrown/Endpoint): Logging with Serilog built in
* [FluentValidation](https://github.com/QuinntyneBrown/Endpoint): Boilerplate for server side validation built in
* [MediatR](https://github.com/QuinntyneBrown/Endpoint): CQRS
* [Entity Framework Core](https://github.com/QuinntyneBrown/Endpoint): Best practices built in


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
endpoint -n TodoApp -r Todo --properties Title:string,IsComplete:bool -a
```

Generates a CRUD Minimal api for a Todo entity

```
endpoint
```

Generates a Default CRUD Api for a Foo entity using the classic ASP.NET structure

```
endpoint -m
```

Generates a Default CRUD Api for a Foo entity using the multiple project Clean Architecture structure
