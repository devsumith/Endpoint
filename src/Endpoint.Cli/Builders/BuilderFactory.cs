using Endpoint.Cli.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Endpoint.Cli.Builders
{
    public static class BuilderFactory
    {
        public static T Create<T>(
            string supportSettingFileNames,
            Func<ICommandService, ITokenBuilder, ITemplateProcessor, ITemplateLocator, IFileSystem, INamingConventionConverter, T> builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>() {
                    { "settingFileNames", supportSettingFileNames }
                })
                .Build();
            var commandService = new CommandService();
            var namingConventionConverter = new NamingConventionConverter();
            var fileSystem = new FileSystem();
            var settingsProvider = new SettingsProvider(fileSystem, configuration);

            var tokenBuilder = new TokenBuilder(namingConventionConverter, settingsProvider, new NamespaceProvider(settingsProvider));
            var templateProcessor = new LiquidTemplateProcessor();
            var templateLocator = new TemplateLocator();

            return builder(commandService, tokenBuilder, templateProcessor, templateLocator, fileSystem, namingConventionConverter);
        }

        public static T Create<T>(
            Func<ICommandService, ITemplateProcessor, ITemplateLocator, IFileSystem, T> builder)
            => builder(new CommandService(), new LiquidTemplateProcessor(), new TemplateLocator(), new FileSystem());
    }
}
