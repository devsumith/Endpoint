// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core;
using Endpoint.Core.Syntax;

namespace Endpoint.Core.Builders
{
    public class FieldBuilder
    {
        private StringBuilder _string;
        private AccessModifier _accessModifier;
        private int _indent;
        private bool _readonly;
        private string _name;
        private string _type;

        public FieldBuilder(string type, string name)
        {
            _string = new();
            _accessModifier = AccessModifier.Inherited;
            _readonly = false;
            _type = type;
            _name = name;
            _indent = 0;
        }

        public FieldBuilder WithIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public FieldBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            _accessModifier = accessModifier;
            return this;
        }

        public FieldBuilder WithReadonly(bool isreadonly = true)
        {
            _readonly = isreadonly;
            return this;
        }

        public string Build()
        {
            if (_accessModifier != AccessModifier.Inherited)
            {
                _string.Append(_accessModifier switch
                {
                    AccessModifier.Private => "private",
                    AccessModifier.Public => "public",
                    _ => throw new System.NotImplementedException()
                });

                _string.Append(' ');
            }

            if (_readonly)
            {
                _string.Append("readonly");
                _string.Append(' ');
            }

            _string.Append(_type);

            _string.Append(' ');

            _string.Append(_accessModifier == AccessModifier.Private ? $"_{_name}" : _name);

            _string.Append(';');

            return _string.ToString().Indent(_indent);
        }
    }
}

