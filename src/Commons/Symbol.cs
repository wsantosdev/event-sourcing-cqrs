using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Commons
{
    public sealed class Symbol : ValueWrapper<string>
    {
        public static readonly Symbol Empty = string.Empty;

        public Symbol(string value) : base(value) { }

        public static implicit operator Symbol(string value) =>
            new(value);

        public static implicit operator string(Symbol symbol) =>
            symbol.Value;

        public static bool operator ==(Symbol left, string right) =>
            left.Value == right;

        public static bool operator !=(Symbol left, string right) =>
            left.Value != right;

        public override bool Equals(object? obj) =>
            base.Equals(obj);

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
