dotnet tool uninstall -g Quinntyne.Endpoint.Cli
dotnet pack ../src/Endpoint.Cli/Endpoint.Cli.csproj
dotnet tool install --global --add-source ../src/Endpoint.Cli/nupkg Quinntyne.Endpoint.Cli --version 0.1.9
