[![NuGet](https://buildstats.info/nuget/Allagi.Endpoint.Cli?includePreReleases=true)](http://www.nuget.org/packages/Allagi.Endpoint.Cli "Download Allagi.Endpoint.Cli from NuGet")

# Endpoint

Model Driven Source Code Generator to build production grade Clean Architecture API's using ASP.NET Core.

## How to run

1. [Download and install the .NET Core SDK](https://dotnet.microsoft.com/download)
2. Open a terminal such as **PowerShell**, **Command Prompt**, or **bash**
3. Execute the Powershell script or Run the following commands:
```sh
dotnet tool install --global Allagi.Endpoint.Cli --version 0.1.0
endpoint -r {YOUR_AGGREGATE_NAME_GOES_HERE} --properties {PROPERTIES_ON_YOUR_AGGREGATE_GOES_HERE} -m
```
Example Command:
endpoint -r Product --properties Name:string -m


