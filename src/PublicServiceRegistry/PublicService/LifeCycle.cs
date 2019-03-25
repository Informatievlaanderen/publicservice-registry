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
        private PublicServiceId _publicServiceId;
        private int _lastUsedId = 0;

        public LifeCycle(Action<object> applyChange) : base(applyChange)
        {
            _lifeCycleStagePeriods = new Dictionary<int, LifeCycleStagePeriod>();

            Register<PublicServiceWasRegistered>(When);
            Register<StageWasAddedToLifeCycle>(When);
            Register<PeriodOfLifeCycleStageWasChanged>(When);
        }

        private void When(StageWasAddedToLifeCycle @event)
        {
            _lastUsedId++;

            var lifeCycleStagePeriod = new LifeCycleStagePeriod(new ValidFrom(@event.From), new ValidTo(@event.To));

            _lifeCycleStagePeriods[@event.Id] = lifeCycleStagePeriod;
        }

        private void When(PeriodOfLifeCycleStageWasChanged @event)
        {
            var lifeCycleStagePeriod = new LifeCycleStagePeriod(new ValidFrom(@event.From), new ValidTo(@event.To));

            _lifeCycleStagePeriods[@event.Id] = lifeCycleStagePeriod;
        }

        private void When(PublicServiceWasRegistered @event)
        {
            _publicServiceId = new PublicServiceId(@event.PublicServiceId);
        }

        public void AddStage(LifeCycleStage lifeCycleStage, LifeCycleStagePeriod period)
        {
            if (_lifeCycleStagePeriods.Any(pair => pair.Value.OverlapsWith(period)))
                throw new LifeCycleCannotHaveOverlappingPeriods();

            Apply(
                new StageWasAddedToLifeCycle(
                    _publicServiceId,
                    ++_lastUsedId,
                    lifeCycleStage,
                    period));
        }

        public void ChangePeriod(int localId, LifeCycleStagePeriod period)
        {
            if (_lifeCycleStagePeriods
                .Where(pair => pair.Key != localId)
                .Any(pair => pair.Value.OverlapsWith(period)))
                throw new LifeCycleCannotHaveOverlappingPeriods();

            Apply(
                new PeriodOfLifeCycleStageWasChanged(
                    _publicServiceId,
                    localId,
                    period));
        }

        private LifeCycleStagePeriod GetPeriodFor(int localId)
        {
            return _lifeCycleStagePeriods.ContainsKey(localId) ? _lifeCycleStagePeriods[localId] : null;
        }
    }
}
