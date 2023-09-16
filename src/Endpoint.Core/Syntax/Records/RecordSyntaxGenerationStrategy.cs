using System.Text;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Syntax.Records.RecordType;

namespace Endpoint.Core.Syntax.Records;

public class RecordSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<RecordModel>
{
    private readonly ILogger<RecordSyntaxGenerationStrategy> logger;

    public RecordSyntaxGenerationStrategy(
        ILogger<RecordSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, RecordModel model)
    {
        logger.LogInformation("Generating Record. {name}", model.Name);

        var sb = new StringBuilder();

        sb.AppendLine($"public record {model.Type switch { Struct => "struct", Class => "class" } } {model.Name}");

        sb.AppendLine("{");

        sb.AppendLine("}");

        return sb.ToString();
    }
}