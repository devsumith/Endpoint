// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.WebApplications;

public class WebApplicationBuilderModel
{
    public WebApplicationBuilderModel(string title, string dbContextName)
    {
        Title = title;
        DbContextName = dbContextName;
    }

    public string Title { get; set; }

    public string DbContextName { get; set; }
}
