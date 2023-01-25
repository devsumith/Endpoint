using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax;

public class SyntaxReferenceSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<SyntaxReferenceModel>
{
    public static string SetInitialLanguageInAppComponent = nameof(SetInitialLanguageInAppComponent);

    private readonly ILogger<SyntaxReferenceSyntaxGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;

    public SyntaxReferenceSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem,
        ILogger<SyntaxReferenceSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override int Priority => 10;

    public override bool CanHandle(object model, dynamic context = null)
        => model is SyntaxReferenceModel && context != null && context.Request == SetInitialLanguageInAppComponent;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, SyntaxReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var tabSize = 2;

        var lines = model.Syntax.Split(Environment.NewLine);

        var createConstructor = !model.Syntax.Contains("constructor");

        var ctor = new string[]
        {
            "constructor(private readonly _translateService: TranslateService) {",
            $"_translateService.setDefaultLang(\"en\");".Indent(1,tabSize),
            $"_translateService.use(localStorage.getItem(\"currentLanguage\") || \"en\");".Indent(1,tabSize),
            "}"
        };

        var importAdded = false;

        var constructorUpdated = false;

        foreach (var line in lines)
        {
            if (!line.StartsWith("import") && !importAdded)
            {
                builder.AppendLine("import { TranslateService } from '@ngx-translate/core';");

                importAdded = true;
            }

            builder.AppendLine(line);

            if (line.StartsWith("export class AppComponent {") && !constructorUpdated && createConstructor)
            {
                foreach (var newLine in ctor)
                {
                    builder.AppendLine(newLine.Indent(1, tabSize));
                }

                constructorUpdated = true;
            }

            if (line.Contains("constructor") && !constructorUpdated && !createConstructor)
            {
                //newLines.RemoveAt(newLines.Count - 1);

                var newLine = line.Replace("constructor(", "constructor(private readonly _translateService: TranslateService, ");

                builder.AppendLine(newLine);

                builder.AppendLine($"_translateService.setDefaultLang(\"en\");".Indent(2, tabSize));
                builder.AppendLine($"_translateService.use(localStorage.getItem(\"currentLanguage\") || \"en\");".Indent(2, tabSize));

                constructorUpdated = true;
            }
        }


        return builder.ToString();
    }
}