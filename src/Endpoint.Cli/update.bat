dotnet tool uninstall -g Endpoint.Cli
dotnet pack
dotnet tool install --global --add-source ./nupkg Endpoint.Cli