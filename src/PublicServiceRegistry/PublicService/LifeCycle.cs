namespace PublicServiceRegistry.PublicService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Events;
    using Exceptions;

    public class LifeCycle : Entity
    {
        private readonly Dictionary<int, LifeCycleStagePeriod> _lifeCycleStagePeriods;
        private LifeCycleStageId _lastUsedId = LifeCycleStageId.Zero();

        private PublicServiceId _publicServiceId;

        public LifeCycle(Action<object> applyChange) : base(applyChange)
        {
            _lifeCycleStagePeriods = new Dictionary<int, LifeCycleStagePeriod>();

            Register<PublicServiceWasRegistered>(When);
            Register<StageWasAddedToLifeCycle>(When);
            Register<PeriodOfLifeCycleStageWasChanged>(When);
            Register<LifeCycleStageWasRemoved>(When);
        }

        private void When(StageWasAddedToLifeCycle @event)
        {
            _lastUsedId = LifeCycleStageId.FromNumber(@event.LifeCycleStageId);

            var lifeCycleStagePeriod = new LifeCycleStagePeriod(new ValidFrom(@event.From), new ValidTo(@event.To));

            _lifeCycleStagePeriods[@event.LifeCycleStageId] = lifeCycleStagePeriod;
        }

        private void When(PeriodOfLifeCycleStageWasChanged @event)
        {
            var lifeCycleStagePeriod = new LifeCycleStagePeriod(new ValidFrom(@event.From), new ValidTo(@event.To));

            _lifeCycleStagePeriods[@event.LifeCycleStageId] = lifeCycleStagePeriod;
        }

        private void When(LifeCycleStageWasRemoved @event)
        {
            _lifeCycleStagePeriods.Remove(@event.LifeCycleStageId);
        }

        private void When(PublicServiceWasRegistered @event)
        {
            _publicServiceId = new PublicServiceId(@event.PublicServiceId);
        }

        public void AddStage(LifeCycleStageType lifeCycleStageType, LifeCycleStagePeriod period)
        {
            if (_lifeCycleStagePeriods.Any(pair => pair.Value.OverlapsWith(period)))
                throw new LifeCycleCannotHaveOverlappingPeriods();

            Apply(
                new StageWasAddedToLifeCycle(
                    _publicServiceId,
                    _lastUsedId.Next(),
                    lifeCycleStageType,
                    period));
        }

        public void ChangePeriod(LifeCycleStageId lifeCycleStageId, LifeCycleStagePeriod period)
        {
            if (_lifeCycleStagePeriods
                .Where(pair => pair.Key != lifeCycleStageId)
                .Any(pair => pair.Value.OverlapsWith(period)))
                throw new LifeCycleCannotHaveOverlappingPeriods();

            Apply(
                new PeriodOfLifeCycleStageWasChanged(
                    _publicServiceId,
                    lifeCycleStageId,
                    period));
        }

        public void RemoveStage(LifeCycleStageId lifeCycleStageId)
        {
            if (!_lifeCycleStagePeriods.ContainsKey(lifeCycleStageId))
                throw new LifeCycleStageWithGivenIdNotFound(lifeCycleStageId);

            Apply(
                new LifeCycleStageWasRemoved(
                    _publicServiceId,
                    lifeCycleStageId));
        }
    }
}
