using Microsoft.Extensions.Logging;
using System.Text;
using static Endpoint.Core.Syntax.Records.RecordType;

namespace Endpoint.Core.Syntax.Records;

public class RecordSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<RecordModel>
{
    private readonly ILogger<RecordSyntaxGenerationStrategy> _logger;

    public RecordSyntaxGenerationStrategy(
        ILogger<RecordSyntaxGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, RecordModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating Record. {name}", model.Name);

        var sb = new StringBuilder();

        sb.AppendLine($"public record {model.Type switch { Struct => "struct", Class => "class" }} {model.Name}");

        sb.AppendLine("{");

        sb.AppendLine("}");

        return sb.ToString();
    }
}