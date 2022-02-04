using Endpoint.SharedKernal.Services;
using System;

namespace Endpoint.Application.Builders
{
    public static class BuilderFactory
    {
        public static T Create<T>(
            Func<ICommandService, ITemplateProcessor, ITemplateLocator, IFileSystem, T> builder)
            => builder(new CommandService(), new LiquidTemplateProcessor(), new TemplateLocator(), new FileSystem());

        public static TTo Map<TFrom, TTo>(TFrom from, TTo to)
            where TTo : BuilderBase<TTo>
            where TFrom : BuilderBase<TFrom>
        {

            return to;
        }
    }
}
