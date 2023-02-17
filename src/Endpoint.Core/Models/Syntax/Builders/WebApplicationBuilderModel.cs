// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.Syntax.Builders;

public class WebApplicationBuilderModel
{
    public WebApplicationBuilderModel(string dbContextName)
    {
        DbContextName = dbContextName;
    }

    public string DbContextName { get; init; }
}

