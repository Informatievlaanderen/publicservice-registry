import moment from 'moment';

import api from 'services/dienstverleningen';

import alerts from 'store/alerts';
import {
  SET_ALERT,
  LOADING_OFF,
  LOADING_ON,
} from 'store/mutation-types';

import success from 'store/successes';

import {
  ChangePeriodForLifeCycleStage,
  RemoveLifeCycleStage,
} from 'services/requests';

const SET_CURRENT_LIFECYCLESTAGE = 'SET_CURRENT_LIFECYCLESTAGE';

const RECEIVE_SORTING = 'RECEIVE_SORTING';
const RECEIVE_PAGING = 'RECEIVE_PAGING';

const SET_MYSERVICE_LIFECYCLE = 'SET_MYSERVICE_LIFECYCLE';
const REMOVE_LIFECYCLESTAGE = 'REMOVE_LIFECYCLESTAGE';

const OBSERVE_LOP = 'OBSERVE_LOP';

function commitRoot(commit, type, payload) {
  commit(type, payload, { root: true });
}

function formatOptionalDate(date) {
  if (!date || date === '') {
    return date;
  }

  return moment(date).format('DD.MM.YYYY');
}

const initialState = {
  currentLifeCycleStage: {
    lifeCycleStageType: '',
    from: '',
    to: '',
  },
  lifeCycle: [],
  listProperties: {
    sorting: {
      field: '',
      direction: 'ascending',
    },
    paging: {
      offset: 0,
      totalItems: 0,
      limit: 10,
    },
  },
  lop: 0,
};

// getters
const getters = {
  sortColumn: (state) => {
    const sorting = state.listProperties.sorting;
    return {
      sortField: sorting.field,
      direction: sorting.direction,
    };
  },
  paging: state => state.listProperties.paging,
  lifeCycle: state => state.lifeCycle || [],
  lifeCycleStage: state => state.lifeCycleStage,
  currentLifeCycleStage: state => state.currentLifeCycleStage,
  numberOfLifeCycleStages: state => (state.lifeCycle || []).length,
};

const mutations = {
  [RECEIVE_SORTING](state, receivedSorting = {}) {
    const sorting = {
      field: receivedSorting.sortBy || '',
      direction: receivedSorting.sortOrder || 'ascending',
    };

    state.listProperties = {
      ...state.listProperties,
      sorting,
    };
  },
  [RECEIVE_PAGING](state, pagingPayload = {}) {
    const paging = {
      offset: pagingPayload.offset || 0,
      totalItems: pagingPayload.totalItems || 0,
      limit: pagingPayload.limit || 10,
    };

    state.listProperties = {
      ...state.listProperties,
      paging,
    };
  },
  [SET_CURRENT_LIFECYCLESTAGE](state, lifeCycleStage) {
    state.currentLifeCycleStage.lifeCycleStageType = lifeCycleStage.levensloopfaseType;
    state.currentLifeCycleStage.from = formatOptionalDate(lifeCycleStage.van);
    state.currentLifeCycleStage.to = formatOptionalDate(lifeCycleStage.tot);
  },
  [SET_MYSERVICE_LIFECYCLE](state, lifeCycle) {
    state.lifeCycle = lifeCycle;
  },
  [REMOVE_LIFECYCLESTAGE](state, lifeCycleStageId) {
    state.lifeCycle = state.lifeCycle.filter(x => x.localId !== lifeCycleStageId);
  },
  [OBSERVE_LOP](state, lop) {
    state.lop = lop;
  },
};

export default class {
  actions;
  namespaced = true;
  state = initialState;
  getters = getters;
  mutations = mutations;
  constructor() {
    this.actions = {

      loadLifeCycle({ commit, state }, payload = {}) {
        commit(SET_MYSERVICE_LIFECYCLE, []);
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycle(payload.id,
          payload.sortOrder || state.listProperties.sorting,
          payload.paging || state.listProperties.paging,
          payload.lop || state.lop)
          .then(({ data, headers }) => {
            commit(SET_MYSERVICE_LIFECYCLE, data);
            commit(RECEIVE_SORTING, JSON.parse(headers['x-sorting'] || null));
            commit(RECEIVE_PAGING, JSON.parse(headers['x-pagination'] || null));
          }).catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          }).finally(() => {
            commitRoot(commit, LOADING_OFF);
          });
      },

      loadLifeCycleStage({ commit }, { publicServiceId, lifeCycleStageId }) {
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycleStage(publicServiceId, lifeCycleStageId)
          .then(lifeCycleStage => commit(SET_CURRENT_LIFECYCLESTAGE, lifeCycleStage.data))
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          }).finally(() => {
            commitRoot(commit, LOADING_OFF);
          });
      },

      addStageToLifeCycle({ commit }, { params: { id }, data }) {
        commitRoot(commit, LOADING_ON);

        api
          .addStageToLifeCycle(id, data)
          .then(() => {
            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      changePeriodForLifeCycleStage({ commit }, { params: { id, localId }, data }) {
        commitRoot(commit, LOADING_ON);

        const request = new ChangePeriodForLifeCycleStage(id, localId, data);

        return api.changePeriodOfLifeCycleStage(request)
          .then(() => {
            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      removeLifeCycleStage({ commit }, { params: { id }, lifeCycleStageId }) {
        commitRoot(commit, LOADING_ON);

        const request = new RemoveLifeCycleStage(id, lifeCycleStageId);

        return api.removeLifeCycleStage(request)
          .then((result) => {
            commit(OBSERVE_LOP, result.data);
            commit(REMOVE_LIFECYCLESTAGE, lifeCycleStageId);
            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },
    };
  }
}
