dotnet tool uninstall -g Allagi.Endpoint.Cli
dotnet pack
dotnet tool install --global --add-source ./nupkg Allagi.Endpoint.Cli