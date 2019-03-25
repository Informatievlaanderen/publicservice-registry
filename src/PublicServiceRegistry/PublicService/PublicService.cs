namespace PublicServiceRegistry.PublicService
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Events;
    using Exceptions;

    public partial class PublicService : AggregateRootEntity
    {
        public static readonly Func<PublicService> Factory = () => new PublicService();

        public static PublicService Register(
            PublicServiceId id,
            PublicServiceName name,
            PrivateZoneId privateZoneId)
        {
            var publicService = Factory();
            publicService.ApplyChange(new PublicServiceWasRegistered(id, name, privateZoneId));
            return publicService;
        }

        public void Rename(PublicServiceName newName)
        {
            EnsureNotRemoved();

            if (newName != _name)
                ApplyChange(new PublicServiceWasRenamed(_id, newName));
        }

        public void AssignCompetentAuthority(Organisation organisation)
        {
            EnsureNotRemoved();

            if (organisation.OvoNumber != _competentAuthorityOvoNumber)
                ApplyChange(
                    new CompetentAuthorityWasAssigned(
                        _id,
                        organisation.OvoNumber,
                        organisation.Name,
                        organisation.Provenance));
        }

        public void UpdateLabels(IDictionary<LabelType, LabelValue> updatedLabels)
        {
            EnsureNotRemoved();

            if (updatedLabels == null)
                return;

            foreach (var label in updatedLabels)
            {
                if (!_labels.ContainsKey(label.Key) || _labels[label.Key] != label.Value)
                {
                    ApplyChange(
                        new LabelWasAssigned(
                            _id,
                            label.Key,
                            label.Value));
                }
            }
        }

        public void AddStageToLifeCycle(LifeCycleStage lifeCycleStage, LifeCycleStagePeriod period)
        {
            EnsureNotRemoved();

            _lifeCycle.AddStage(lifeCycleStage, period);
        }

        public void ChangePeriodOfLifeCycleStage(int localId, LifeCycleStagePeriod period)
        {
            EnsureNotRemoved();

            _lifeCycle.ChangePeriod(localId, period);
        }

        public void MarkForOrafinExport(bool exportToOrafin)
        {
            EnsureNotRemoved();

            if (exportToOrafin != _exportToOrafin)
                ApplyChange(new OrafinExportPropertyWasSet(_id, exportToOrafin));
        }

        public void Remove(ReasonForRemoval reasonForRemoval)
        {
            EnsureNotRemoved();

            ApplyChange(new PublicServiceWasRemoved(_id, reasonForRemoval));
        }

        private void EnsureNotRemoved()
        {
            if (IsRemoved)
                throw new CannotPerformActionOnRemovedPublicService();
        }
    }
}
