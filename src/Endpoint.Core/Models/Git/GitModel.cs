using System;

namespace Endpoint.Core.Models.Git
{
    public class GitModel
    {
        public string Username { get; set; } = Environment.GetEnvironmentVariable("Endpoint:GitUsername");
        public string Email { get; set; } = Environment.GetEnvironmentVariable("Endpoint:GitEmail");
        public string PersonalAccessToken { get; set; } = Environment.GetEnvironmentVariable("Endpoint:GitPassword");
        public string RepositoryName { get; set; }
        public string Directory { get; set; } = Environment.CurrentDirectory;
    }
}
