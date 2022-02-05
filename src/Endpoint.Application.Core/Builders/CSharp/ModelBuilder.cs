using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class ModelBuilder
    {


        public void Build()
        {
/*            new ClassBuilder(_entityName.PascalCase, _context, _fileSystem)
                .WithDirectory($"{_domainDirectory.Value}{Path.DirectorySeparatorChar}Models")
                .WithUsing("System")
                .WithNamespace($"{_domainNamespace.Value}.Models")
                .WithProperty(new PropertyBuilder().WithName($"{_entityName.PascalCase}Id").WithType("Guid").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Build();*/
        }
    }
}
