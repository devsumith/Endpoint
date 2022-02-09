using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System;
using Endpoint.SharedKernal;

namespace Endpoint.Application.Builders
{
    public class QueryBuilder
    {

        public void Build()
        {

            /*            var template = _templateLocator.Get(nameof(QueryBuilder));

                        var tokens = new TokensBuilder()
                            .With(nameof(_entityName), _entityName)
                            .With(nameof(_name), _name)
                            .With(nameof(_applicationNamespace), _applicationNamespace)
                            .With(nameof(_directory), _directory)
                            .With(nameof(_domainNamespace), _domainNamespace)
                            .With(nameof(_dbContext), _dbContext)
                            .Build();

                        var contents = _templateProcessor.Process(template, tokens);

                        _fileSystem.WriteAllLines($@"{_applicationDirectory.Value}/Features/{_entityName.PascalCasePlural}/{_name.PascalCase}.cs", contents);*/

        }
    }
}
