namespace PublicServiceRegistry.PublicService
{
    using System.Collections.Generic;
    using Events;

    public partial class PublicService
    {
        private PublicServiceId _id;
        private PublicServiceName _name;
        private OvoNumber _competentAuthorityOvoNumber;
        private bool _exportToOrafin;
        private IpdcCode _ipdcCode;

        private readonly Dictionary<LabelType, LabelValue> _labels;
        private readonly LifeCycle _lifeCycle;

        private bool IsRemoved { get; set; }
        private bool IsIpdcCodeSet => _ipdcCode != null;

        private PublicService()
        {
            _labels = new Dictionary<LabelType, LabelValue>();
            _lifeCycle = new LifeCycle(ApplyChange);

            Register<PublicServiceWasRegistered>(When);
            Register<PublicServiceWasRenamed>(When);
            Register<CompetentAuthorityWasAssigned>(When);
            Register<OrafinExportPropertyWasSet>(When);
            Register<LabelWasAssigned>(When);
            Register<StageWasAddedToLifeCycle>(When);
            Register<PeriodOfLifeCycleStageWasChanged>(When);
            Register<LifeCycleStageWasRemoved>(When);
            Register<IpdcCodeWasSet>(When);
            Register<PublicServiceWasRemoved>(When);
        }

        private void When(PublicServiceWasRegistered @event)
        {
            _id = new PublicServiceId(@event.PublicServiceId);
            _name = new PublicServiceName(@event.Name);

            _lifeCycle.Route(@event);
        }

        private void When(PublicServiceWasRenamed @event)
        {
            _name = new PublicServiceName(@event.NewName);
        }

        private void When(CompetentAuthorityWasAssigned @event)
        {
            _competentAuthorityOvoNumber = new OvoNumber(@event.CompetentAuthorityCode);
        }

        private void When(OrafinExportPropertyWasSet @event)
        {
            _exportToOrafin = @event.ExportToOrafin;
        }

        private void When(LabelWasAssigned @event)
        {
            var labelType = LabelType.Parse(@event.LabelType);
            _labels[labelType] = new LabelValue(@event.LabelValue);
        }

        private void When(StageWasAddedToLifeCycle @event)
        {
            _lifeCycle.Route(@event);
        }

        private void When(PeriodOfLifeCycleStageWasChanged @event)
        {
            _lifeCycle.Route(@event);
        }

        private void When(LifeCycleStageWasRemoved @event)
        {
            _lifeCycle.Route(@event);
        }

        private void When(IpdcCodeWasSet @event)
        {
            _ipdcCode = new IpdcCode(@event.IpdcCode);
        }

        private void When(PublicServiceWasRemoved @event)
        {
            IsRemoved = true;
        }
    }
}
