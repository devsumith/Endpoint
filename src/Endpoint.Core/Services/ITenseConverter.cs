// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Services;

public interface ITenseConverter
{
    string Convert(string value, bool pastTense = true);
}

