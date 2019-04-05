import moment from 'moment';

function formatDate(date) {
  if (!date || date === '') {
    return '';
  }

  return moment(date, 'DD.MM.YYYY').format('YYYY-MM-DD');
}

export class AddStageToLifeCycle {
  constructor(
    publicServiceId,
    data) {
    this.publicServiceId = publicServiceId;

    this.body = {
      levensloopfaseType: data.lifeCycleStageType,
      vanaf: formatDate(data.from),
      tot: formatDate(data.to),
    };
  }
}

export class ChangePeriodForLifeCycleStage {
  constructor(
    publicServiceId,
    lifeCycleStageId,
    data) {
    this.publicServiceId = publicServiceId;
    this.lifeCycleStageId = lifeCycleStageId;

    this.body = {
      vanaf: formatDate(data.from),
      tot: formatDate(data.to),
    };
  }
}

export class RemoveLifeCycleStage {
  constructor(
    publicServiceId,
    lifeCycleStageId) {
    this.publicServiceId = publicServiceId;
    this.lifeCycleStageId = lifeCycleStageId;
  }
}


export default {
};
