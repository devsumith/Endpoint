// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Classes;

public class RequestValidatorModel : ClassModel
{
    public List<RuleForModel> Rules { get; set; }
    public RequestValidatorModel(RequestModel request)
        : base($"{request.Name}Validator")
    {
        
        Implements.Add(new TypeModel("AbstractValidator")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(request.Name) {}
            }
        });

        var ctor = new ConstructorModel(this, Name);

        var ctorBody = new StringBuilder();

        ctorBody.AppendLine();

        foreach (var property in request.Properties)
        {
            ctorBody.AppendLine(property.Type.Name switch
            {
                "Guid" => $"RuleFor(x => x.{property.Name}).NotEqual(default(Guid));",
                _ => $"RuleFor(x => x.{property.Name}).NotNull();"
            });
        }

        ctor.Body = ctorBody.ToString();

        Constructors.Add(ctor);

    }
}

