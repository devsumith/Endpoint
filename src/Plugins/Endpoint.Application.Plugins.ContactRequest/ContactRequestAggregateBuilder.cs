using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using System.Collections.Generic;
using static Endpoint.SharedKernal.Models.ClassPropertyAccessor;

namespace Endpoint.Application.Plugins.ContactRequest.Builders
{
    public class ContactRequestAggregateBuilder
    {
        public static void Build(Settings settings, IFileSystem fileSystem)
        {
            Core.Builders.Core.AggregateBuilder.Build("ContactRequest", new List<ClassProperty>() {
                new ("public", "string", "RequestedByEmail", GetPrivateSet),
                new ("public", "string", "RequestedByName ", GetPrivateSet),
                new ("public", "string", "Description ", GetPrivateSet)
            }, settings, fileSystem);
        }

    }
}
