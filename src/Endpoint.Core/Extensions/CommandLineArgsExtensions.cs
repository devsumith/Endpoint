// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using System;
using System.Linq;

namespace Endpoint.Core.Extensions;

public static class CommandLineArgsExtensions { 

    public static object ParseArguments(this string[] args)
    {
        var parser = new Parser(with =>
        {
            with.CaseSensitive = false;
            with.HelpWriter = Console.Out;
            with.IgnoreUnknownArguments = true;
        });

        if (args.Length == 0 || args[0].StartsWith('-'))
        {
            args = new string[1] { "default" }.Concat(args).ToArray();
        }

        var verbs = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
            .ToArray();

        var parsedResult = parser.ParseArguments(args, verbs);

        if (parsedResult.Errors.SingleOrDefault() is HelpRequestedError || parsedResult.Errors.SingleOrDefault() is HelpRequestedError)
        {
            Environment.Exit(0);
        }

        if (parsedResult.Errors.SingleOrDefault() is BadVerbSelectedError error)
        {
            throw new Exception($"{error.Tag}:{error.Token}");
        }

        return parsedResult.Value;
    }
}
