// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Endpoint.Core.Internals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax;

public class UtlitityService : IUtlitityService
{
    private readonly ILogger<UtlitityService> logger;
    private readonly Observable<INotification> observableNotifications;

    public UtlitityService(
        ILogger<UtlitityService> logger,
        Observable<INotification> observableNotifications)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.observableNotifications = observableNotifications ?? throw new ArgumentNullException(nameof(observableNotifications));
    }

    public void CopyrightAdd(string directory)
    {
        foreach (var path in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
        {
            var ignore = path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}");

            var extension = Path.GetExtension(path);

            var validExtension = extension == ".cs" || extension == ".ts" || extension == ".js";

            if (validExtension && !ignore)
            {
                logger.LogInformation("Broadcasting FileCreated Notification for {path}", path);

                // TODO: write the copyright
            }
        }
    }
}
