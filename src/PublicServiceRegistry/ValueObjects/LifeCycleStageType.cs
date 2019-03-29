namespace PublicServiceRegistry
{
    using System;

    public class LifeCycleStageType : IEquatable<LifeCycleStageType>
    {
        private readonly string _value;

        public static readonly LifeCycleStageType Active = new LifeCycleStageType(nameof(Active));
        public static readonly LifeCycleStageType UnderDevelopment = new LifeCycleStageType(nameof(UnderDevelopment));
        public static readonly LifeCycleStageType PhasingOut = new LifeCycleStageType(nameof(PhasingOut));
        public static readonly LifeCycleStageType Stopped = new LifeCycleStageType(nameof(Stopped));

        public static readonly LifeCycleStageType[] All =
        {
            Active,
            UnderDevelopment,
            PhasingOut,
            Stopped,
        };

        private LifeCycleStageType(string value)
        {
            _value = value;
        }

        public static bool CanParse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Array.Find(All, candidate => candidate._value == value) != null;
        }

        public static bool TryParse(string value, out LifeCycleStageType parsed)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            parsed = Array.Find(All, candidate => candidate._value == value);
            return parsed != null;
        }

        public static LifeCycleStageType Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!TryParse(value, out var parsed))
                throw new FormatException($"The identifier {value} does not correspond to any life cycle stages.");

            return parsed;
        }

        public bool Equals(LifeCycleStageType other) => other != null && other._value == _value;
        public override bool Equals(object obj) => obj is LifeCycleStageType type && Equals(type);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value;

        public static implicit operator string(LifeCycleStageType instance) => instance.ToString();
        public static bool operator ==(LifeCycleStageType left, LifeCycleStageType right) => Equals(left, right);
        public static bool operator !=(LifeCycleStageType left, LifeCycleStageType right) => !Equals(left, right);
    }

}
