namespace PublicServiceRegistry
{
    using System;

    public class LifeCycleStage : IEquatable<LifeCycleStage>
    {
        private readonly string _value;

        public static readonly LifeCycleStage Active = new LifeCycleStage(nameof(Active));
        public static readonly LifeCycleStage UnderDevelopment = new LifeCycleStage(nameof(UnderDevelopment));
        public static readonly LifeCycleStage PhasingOut = new LifeCycleStage(nameof(PhasingOut));
        public static readonly LifeCycleStage Stopped = new LifeCycleStage(nameof(Stopped));

        public static readonly LifeCycleStage[] All =
        {
            Active,
            UnderDevelopment,
            PhasingOut,
            Stopped,
        };

        private LifeCycleStage(string value)
        {
            _value = value;
        }

        public static bool CanParse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Array.Find(All, candidate => candidate._value == value) != null;
        }

        public static bool TryParse(string value, out LifeCycleStage parsed)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            parsed = Array.Find(All, candidate => candidate._value == value);
            return parsed != null;
        }

        public static LifeCycleStage Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!TryParse(value, out var parsed))
                throw new FormatException($"The identifier {value} does not correspond to any life cycle stages.");

            return parsed;
        }

        public bool Equals(LifeCycleStage other) => other != null && other._value == _value;
        public override bool Equals(object obj) => obj is LifeCycleStage type && Equals(type);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value;

        public static implicit operator string(LifeCycleStage instance) => instance.ToString();
        public static bool operator ==(LifeCycleStage left, LifeCycleStage right) => Equals(left, right);
        public static bool operator !=(LifeCycleStage left, LifeCycleStage right) => !Equals(left, right);
    }

}
