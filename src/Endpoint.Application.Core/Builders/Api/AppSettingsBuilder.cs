using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class AppSettingsBuilder
    {
        public void Build()
        {
/*            var template = _templateLocator.Get(nameof(AppSettingsBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/appsettings.json", contents);*/
        }
    }
}
