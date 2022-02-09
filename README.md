# Endpoint

Model Driven Source Code Generator to build production grade Clean Architecture API's using ASP.NET Core.

## How to run locally

0. Clone the repo
1. [Download and install the .NET Core SDK](https://dotnet.microsoft.com/download)
2. Open a terminal such as **PowerShell**, **Command Prompt**, or **bash** and navigate to the `src/Endpoint.Cli/update.ps1` file
3. Execute the Powershell script or Run the following commands in the same folder:
```sh
dotnet tool install --global Allagi.Endpoint.Cli --version 0.1.0
endpoint -r {YOUR_AGGREGATE_NAME_GOES_HERE} --properties {PROPERTIES_ON_YOUR_AGGREGATE_GOES_HERE} -m
```
Example Command:
endpoint -r Product --properties Name:string -m


