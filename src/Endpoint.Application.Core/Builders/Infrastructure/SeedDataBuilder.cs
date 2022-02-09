using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class SeedDataBuilder
    {

        public void Build()
        {
            /*            var template = _templateLocator.Get(nameof(SeedDataBuilder));

                        var tokens = new TokensBuilder()
                            .With(nameof(_infrastructureNamespace), _infrastructureNamespace)
                            .With(nameof(_dbContext), _dbContext)
                            .Build();

                        var contents = _templateProcessor.Process(template, tokens);

                        _fileSystem.WriteAllLines($@"{_infrastructureDirectory.Value}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}SeedData.cs", contents);*/
        }
    }
}
