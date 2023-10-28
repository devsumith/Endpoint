dotnet tool uninstall -g Quinntyne.Endpoint.Cli
dotnet pack -c Release
dotnet tool install --global --add-source ./nupkg Quinntyne.Endpoint.Cli --version 0.2.3
