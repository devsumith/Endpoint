// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Git;

public class GitModel
{
    public GitModel(string repositoryName)
    {
        RepositoryName = repositoryName;
        Username = Environment.GetEnvironmentVariable("Endpoint:GitUsername");
        Email = Environment.GetEnvironmentVariable("Endpoint:GitEmail");
        PersonalAccessToken = Environment.GetEnvironmentVariable("Endpoint:GitPassword");
        Directory = Environment.CurrentDirectory;
    }

    public string Username { get; init; }

    public string Email { get; init; }

    public string PersonalAccessToken { get; init; }

    public string RepositoryName { get; init; }

    public string Directory { get; init; }
}
