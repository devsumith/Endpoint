// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Syntax;
using System.Text;


namespace Endpoint.Core.Builders;

public class ParameterBuilder
{
    private StringBuilder _string;
    private string _from;
    private string _type;
    private string _value;
    private bool _extension;
    public ParameterBuilder(string type, string value, bool extension = false)
    {
        _string = new StringBuilder();
        _type = type;
        _value = value;
        _extension = extension;
    }

    public ParameterBuilder WithFrom(From from)
    {
        _from = from switch
        {
            From.Route => new GenericAttributeGenerationStrategy().WithName("FromRoute").Build(),
            From.Body => new GenericAttributeGenerationStrategy().WithName("FromBody").Build(),
            _ => throw new System.NotImplementedException()
        };

        return this;
    }

    public string Build()
    {
        if (!string.IsNullOrEmpty(_from))
        {
            _string.Append(_from);
        }
        if (_extension)
        {
            _string.Append("this ");
        }

        _string.Append(_type);

        _string.Append(' ');

        _string.Append(_value);

        return _string.ToString();
    }
}


