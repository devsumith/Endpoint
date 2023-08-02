// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax;

public class TsConfigModel
{
    public TsConfigModel()
    {
        Include = new List<string>()
        {
            "**/*.ts"
        };

        CompilerOptions = new TsConfigCompilerOptions();
    }
    public TsConfigCompilerOptions CompilerOptions { get; set; }
    public List<string> Include { get; set; }
}

public class TsConfigCompilerOptions
{
    public TsConfigCompilerOptions()
    {
        Target = "es2018";
        Module = "esnext";
        ModuleResolution = "NodeNext";
        NoEmitOnError = true;
        Lib = new List<string>()
        {
            "es2017","dom"
        };
        Strict = false;
        EsModuleInterop = false;
        AllowSyntheticDefaultImports = true;
        ExperimentalDecorators = true;
        ImportHelpers = true;
        OutDir = "dist";
        SourceMap = true;
        InlineSources = true;
        RootDir = "./";
        Declaration = true;
        Incremental = true;
    }

    public string Target { get; set; }
    public string Module { get; set; }
    public string ModuleResolution { get; set; }
    public bool NoEmitOnError { get; set; }
    public List<string> Lib { get; set; }
    public bool Strict { get; set; }
    public bool EsModuleInterop { get; set; }
    public bool AllowSyntheticDefaultImports { get; set; }
    public bool ExperimentalDecorators { get; set; }
    public bool ImportHelpers { get; set; }
    public string OutDir { get; set; }
    public bool SourceMap { get; set; }
    public bool InlineSources { get; set; }
    public string RootDir { get; set; }
    public bool Declaration { get; set; }
    public bool Incremental { get; set; }

}

