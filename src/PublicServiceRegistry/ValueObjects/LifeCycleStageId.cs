namespace PublicServiceRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class LifeCycleStageId: IntegerValueObject<LifeCycleStageId>
    {
        private LifeCycleStageId([JsonProperty("value")] int lifeCycleStageId) : base(lifeCycleStageId)
        {

        }

        public static LifeCycleStageId Zero()
        {
            return new LifeCycleStageId(0);
        }

        public LifeCycleStageId Next()
        {
            return new LifeCycleStageId(Value + 1);
        }

        public static LifeCycleStageId FromNumber(int value)
        {
            if (value < 1)
                throw new ArgumentException(nameof(value));

            return new LifeCycleStageId(value);
        }
    }
}
