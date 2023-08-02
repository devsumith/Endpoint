// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Core.Syntax;

public class UtlitityService : IUtlitityService
{
    private readonly ILogger<UtlitityService> _logger;
    private readonly Observable<INotification> _observableNotifications;
    public UtlitityService(
        ILogger<UtlitityService> logger,
        Observable<INotification> observableNotifications
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _observableNotifications = observableNotifications ?? throw new ArgumentNullException(nameof(observableNotifications));
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
                _logger.LogInformation("Broadcasting FileCreated Notification for {path}", path);

                _observableNotifications.Broadcast(new FileCreated(path));
            }
        }
    }
}


