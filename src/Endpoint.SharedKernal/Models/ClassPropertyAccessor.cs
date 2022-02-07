using System.Collections.Generic;

namespace Endpoint.SharedKernal.Models
{
    public class ClassPropertyAccessor
    {
        public string AccessModifier { get; private set; }
        public ClassPropertyAccessorType Type { get; private set; }

        public ClassPropertyAccessor(string accessModifier, ClassPropertyAccessorType classPropertyAccessorType)
            :this(classPropertyAccessorType)
        {            
            AccessModifier=accessModifier;
        }

        public ClassPropertyAccessor(ClassPropertyAccessorType classPropertyAccessorType)
        {
            Type = classPropertyAccessorType;
        }

        public static ClassPropertyAccessor Get => new ClassPropertyAccessor(ClassPropertyAccessorType.Get);

        public static ClassPropertyAccessor PrivateSet => new ClassPropertyAccessor("private", ClassPropertyAccessorType.Set);

        public static List<ClassPropertyAccessor> GetPrivateSet => new List<ClassPropertyAccessor>() { Get, PrivateSet };

    }
}
