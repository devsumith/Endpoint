// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Classes;

public class RequestValidatorModel : ClassModel
{
    public List<RuleForModel> Rules { get; set; }
    public RequestValidatorModel(ClassModel request)
        : base($"{request.Name}Validator")
    {
        Rules = new List<RuleForModel>();

        foreach (var property in request.Properties)
        {
            Rules.Add(new RuleForModel(property));
        }

        Implements.Add(new TypeModel("AbstractValidator")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(request.Name) {}
            }
        });
    }

}

