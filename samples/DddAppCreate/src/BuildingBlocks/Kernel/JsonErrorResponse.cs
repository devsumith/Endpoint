// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Kernel;

public partial class HttpGlobalExceptionFilter
{
    private class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMeesage { get; set; }
    }
}
