using CommandLine;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;

Log.Information("Starting Endpoint");

var container = BuildContainer();

var mediator = container.GetService<IMediator>();

var configuration = container.GetRequiredService<IConfiguration>();

ProcessArgs(mediator, configuration, args);

static Parser _createParser()
{
   return new Parser(with =>
   {
       with.CaseSensitive = false;
       with.HelpWriter = Console.Out;
       with.IgnoreUnknownArguments = true;
   });
}

static IServiceProvider BuildContainer()
{
   var host = Host.CreateDefaultBuilder()
       .ConfigureServices((services) =>
       {
           services.AddCliServices();
       })
       .UseSerilog()
       .Build();

   return host.Services;
}

static void ProcessArgs(IMediator mediator, IConfiguration configuration, string[] args)
{
   if (args.Length == 0 || args[0].StartsWith("-"))
   {
       args = new string[1] { configuration[CoreConstants.EnvironmentVariables.DefaultCommand] }.Concat(args).ToArray();
   }

   var verbs = AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(s => s.GetTypes())
       .Where(type => type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
       .ToArray();

   _createParser().ParseArguments(args, verbs)
       .WithParsed(
         (dynamic request) =>
         {

             try
             {
                 mediator.Send(request).GetAwaiter().GetResult();
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         });
}
