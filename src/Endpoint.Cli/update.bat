dotnet tool uninstall -g Quinntyne.Endpoint.Cli
dotnet pack
dotnet tool install --global --add-source ./nupkg Quinntyne.Endpoint.Cli --version 0.1.4
