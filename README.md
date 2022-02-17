# Endpoint
[![NuGet](https://buildstats.info/nuget/Allagi.Endpoint.Cli?includePreReleases=true)](http://www.nuget.org/packages/Allagi.Endpoint.Cli "Download Allagi.Endpoint.Cli from NuGet")

Model Driven Source Code Generator to build production grade Clean Architecture API's using ASP.NET Core.

## Installation

Install the [Endpoint NuGet Package](https://www.nuget.org/packages/Allagi.Endpoint.Cli).


### .NET Core CLI

```
dotnet tool install --global Allagi.Endpoint.Cli
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
