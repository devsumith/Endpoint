// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Documents;

public class AggregateModel : DocumentModel
{
    public ClassModel Aggregate { get; set; }

    public ClassModel AggregateDto { get; set; }

    public ClassModel AggregateExtensions { get; set; }

    public List<CommandModel> Commands { get; set; }

    public List<QueryModel> Queries { get; set; }
}
