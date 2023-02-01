// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace Endpoint.Core.Models.Syntax;

public class UtlitityService: IUtlitityService
{
    private readonly ILogger<UtlitityService> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly IFileSystem _fileSystem;

    public UtlitityService(
        ILogger<UtlitityService> logger,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public void CopyrightAdd(string directory)
    {
        var copyright = string.Join(Environment.NewLine, _templateLocator.Get("Copyright"));

        foreach (var path in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
        {
            var ignore = path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}");

            var extension = Path.GetExtension(path);

            var validExtension = extension == ".cs" || extension == ".ts";

            if (validExtension && !ignore)
            {
                var originalFileContents = _fileSystem.ReadAllText(path);

                if(originalFileContents.Contains(copyright) == false)
                {
                    var newFileContentsBuilder = new StringBuilder();

                    newFileContentsBuilder.AppendLine(copyright);
                    
                    newFileContentsBuilder.AppendLine();

                    newFileContentsBuilder.AppendLine(originalFileContents);

                    _fileSystem.WriteAllText(path, newFileContentsBuilder.ToString());
                }
            }

        }



        

    }
}


