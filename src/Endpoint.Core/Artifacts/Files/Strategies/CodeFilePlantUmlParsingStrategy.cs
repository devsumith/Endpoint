// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Namespaces;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Constants.FileExtensions;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public abstract class CodeFilePlantUmlParsingStrategy<T> : BaseArtifactParsingStrategy<CodeFileModel<T>>
    where T : SyntaxModel
{
    private readonly ILogger<CodeFilePlantUmlParsingStrategy<T>> _logger;
    private readonly IContext _context;
    private readonly ISyntaxParser _syntaxParser;

    public CodeFilePlantUmlParsingStrategy(IContext context, ISyntaxParser syntaxParser, ILogger<CodeFilePlantUmlParsingStrategy<T>> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _syntaxParser = syntaxParser ?? throw new ArgumentNullException(nameof(syntaxParser));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<CodeFileModel<T>> ParseAsync(IArtifactParser parser, string valueOrDirectoryOrPath)
    {
        _logger.LogInformation("Parsing PlantUml for CodeFile. {typeName}", typeof(T).Name);

        var codeFile = _context.Get<CodeFileModel<T>>();

        var model = await _syntaxParser.ParseAsync<T>(valueOrDirectoryOrPath);

        return new CodeFileModel<T>(model, codeFile.Name, codeFile.Directory, CSharpFile);
    }
}

public class NamespaceCodeFilePlantUmlParsingStrategy : CodeFilePlantUmlParsingStrategy<NamespaceModel>
{
    public NamespaceCodeFilePlantUmlParsingStrategy(IContext context, ISyntaxParser syntaxParser, ILogger<CodeFilePlantUmlParsingStrategy<NamespaceModel>> logger) : 
        base(context, syntaxParser, logger)
    { }
}