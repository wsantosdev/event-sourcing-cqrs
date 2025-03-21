namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public class ValueWrapper<T> : IEquatable<T>
    {
        public T Value { get; set; }

        protected ValueWrapper(T value) =>
            Value = value;

        public static bool operator ==(ValueWrapper<T> left, ValueWrapper<T> right) =>
            left.Equals(right);

        public static bool operator !=(ValueWrapper<T> left, ValueWrapper<T> right) =>
            !left.Equals(right);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not ValueWrapper<T> simpleValue)
                return false;

            return Equals(simpleValue.Value);
        }

        public bool Equals(T? otherValue)
        {
            if (otherValue is null)
                return false;

            return Value!.Equals(otherValue);
        }

        public override int GetHashCode() =>
            HashCode.Combine(Value);

        public override string ToString() =>
            Value!.ToString()!;
    }
}