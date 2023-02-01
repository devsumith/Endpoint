// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Services;

public interface INamingConventionConverter
{
    string Convert(NamingConvention from, NamingConvention to, string value);

    string Convert(NamingConvention to, string value);

    string Convert(NamingConvention to, string value, bool pluralize);
}
