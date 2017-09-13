using System;

namespace Clouty.Helpers.PartialUpdate
{
    public class Assignable<T>
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public T Value { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Boolean IsAssigned { get; private set; }

        public void Apply(Action<T> selector)
        {
            if (IsAssigned)
            {
                selector(Value);
            }
        }

        public override String ToString()
        {
            return IsAssigned ? (Value?.ToString() ?? "null") : "NOT ASSIGNED";
        }
    }
}