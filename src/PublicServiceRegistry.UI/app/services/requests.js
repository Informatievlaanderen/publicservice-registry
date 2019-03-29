import moment from 'moment';

function formatDate(date) {
  if (!date || date === '') {
    return '';
  }

  return moment(date, 'DD.MM.YYYY').format('YYYY-MM-DD');
}

export class ChangePeriodForLifeCycleStage {
  constructor(
    publicServiceId,
    lifeCycleStageId,
    data) {
    this.publicServiceId = publicServiceId;
    this.lifeCycleStageId = lifeCycleStageId;

    this.body = {
      van: formatDate(data.from),
      tot: formatDate(data.to),
    };
  }
}

export default {
};
