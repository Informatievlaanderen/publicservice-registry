import moment from 'moment';

function formatDate(date) {
  if (!date || date === '') {
    return '';
  }

  return moment(date, moment.ISO_8601).format('DD.MM.YYYY');
}

export class LifeCycleResponse {
  constructor({ data, headers }) {
    this.lifeCycle = data.map(item => ({
      publicServiceId: item.dienstverleningId,
      lifeCycleStageId: item.levensloopfaseId,
      lifeCycleStageTypeId: item.levensloopfaseTypeId,
      lifeCycleStageTypeName: item.levensloopfaseTypeNaam,
      from: formatDate(item.vanaf),
      to: formatDate(item.tot),
    }));
    this.sorting = JSON.parse(headers['x-sorting'] || null);
    this.pagination = JSON.parse(headers['x-pagination'] || null);
  }
}

export class LifeCycleStageResponse {
  constructor({ data }) {
    this.lifeCycleStage = {
      lifeCycleStageType: data.levensloopfaseType,
      from: formatDate(data.vanaf),
      to: formatDate(data.tot),
    };
  }
}

export default {
};
