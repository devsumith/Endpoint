// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.SystemModels;

public class Property {
    public string Name { get; set; }
    public Type Type { get; set; }
    public bool IsPrimaryKey { get; set; }
}
