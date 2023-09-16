// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.Angular;

public class FileReplacementModel
{
    public FileReplacementModel(string replace, string with)
    {
        Replace = replace;
        With = with;
    }

    public string Replace { get; init; }

    public string With { get; init; }
}
