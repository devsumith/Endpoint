// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Endpoint.Core.Syntax.ExpressionBodies;

public class TemplatedExpressionBodyModel {

	public TemplatedExpressionBodyModel(string template)
	{
		Template = template;
	}
    public string Template { get; set; }
}
