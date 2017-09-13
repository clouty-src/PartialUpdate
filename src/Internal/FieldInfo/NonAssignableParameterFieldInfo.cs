using System;

namespace Clouty.Helpers.PartialUpdate
{
    internal class NonAssignableParameterFieldInfo : ParameterFieldInfo
    {
        private readonly Func<Type, Object> _emptyValueBuilder;

        public Object EmptyValue => _emptyValueBuilder(PropertyType);

        public NonAssignableParameterFieldInfo(Func<Type, Object> emptyValueBuilder)
        {
            _emptyValueBuilder = emptyValueBuilder;
        }
    }
}