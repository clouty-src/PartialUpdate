using System;
using System.Reflection;

namespace Clouty.Helpers.PartialUpdate
{
    internal class AssignableParameterFieldInfo : ParameterFieldInfo
    {
        public Type GenericBaseType { get; set; }

        public PropertyInfo AssignableIsSetPropertyInfo { get; set; }

        public PropertyInfo AssignableValuePropertyInfo { get; set; }
    }
}