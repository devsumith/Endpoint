
using Endpoint.Core.Options;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;


namespace Endpoint.Core.Strategies;

public class AdditionalMinimalApiResourceGenerationStrategy : IAdditionalResourceGenerationStrategy
{
    private readonly ILogger _logger;

    public AdditionalMinimalApiResourceGenerationStrategy(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int Order => 1;

    public bool CanHandle(AddResourceOptions options)
    {
        var path = Directory.GetFiles(options.Directory, "Program.cs", SearchOption.AllDirectories).Single();
        return File.ReadAllLines(path).Where(x => x.StartsWith("app.Map")).Count() > 0;
    }

    public void Create(AddResourceOptions options)
    {
        _logger.LogInformation(nameof(AdditionalMinimalApiResourceGenerationStrategy));

        Console.WriteLine("Press any key to continue...");

        Console.ReadKey();
    }
}

