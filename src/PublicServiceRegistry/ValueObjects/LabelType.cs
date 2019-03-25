namespace PublicServiceRegistry
{
    using System;

    public class LabelType : IEquatable<LabelType>
    {
        private readonly string _value;

        public static readonly LabelType Ipdc = new LabelType(nameof(Ipdc));
        public static readonly LabelType Subsidieregister = new LabelType(nameof(Subsidieregister));

        public static readonly LabelType[] All =
        {
            Ipdc,
            Subsidieregister
        };

        private LabelType(string value) => _value = value;

        public static bool CanParse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Array.Find(All, candidate => candidate._value == value) != null;
        }

        public static bool TryParse(string value, out LabelType parsed)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            parsed = Array.Find(All, candidate => candidate._value == value);
            return parsed != null;
        }

        public static LabelType Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!TryParse(value, out var parsed))
                throw new FormatException($"The identifier {value} does not correspond to any alternative label types.");

            return parsed;
        }

        public bool Equals(LabelType other) => other != null && other._value == _value;
        public override bool Equals(object obj) => obj is LabelType type && Equals(type);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value;

        public static implicit operator string(LabelType instance) => instance.ToString();
        public static bool operator ==(LabelType left, LabelType right) => Equals(left, right);
        public static bool operator !=(LabelType left, LabelType right) => !Equals(left, right);
    }

}
