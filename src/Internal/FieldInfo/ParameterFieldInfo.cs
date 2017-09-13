using System;
using System.Reflection;

namespace Clouty.Helpers.PartialUpdate
{
    internal class ParameterFieldInfo
    {
        public String JsonFieldName { get; set; }

        public Type PropertyType { get; set; }

        public PropertyInfo PropertyInfo { get; set; }
    }
}