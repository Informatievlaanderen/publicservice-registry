import api from 'services/dienstverleningen';

import alerts from 'store/alerts';
import {
  SET_ALERT,
  LOADING_OFF,
  LOADING_ON,
} from 'store/mutation-types';

import success from 'store/successes';

import {
  AddStageToLifeCycle,
  ChangePeriodForLifeCycleStage,
  RemoveLifeCycleStage,
} from 'services/requests';

import {
  LifeCycleResponse,
  LifeCycleStageResponse,
} from 'services/responses';

const SET_CURRENT_LIFECYCLESTAGE = 'SET_CURRENT_LIFECYCLESTAGE';

const RECEIVE_SORTING = 'RECEIVE_SORTING';
const RECEIVE_PAGING = 'RECEIVE_PAGING';

const SET_MYSERVICE_LIFECYCLE = 'SET_MYSERVICE_LIFECYCLE';
const REMOVE_LIFECYCLESTAGE = 'REMOVE_LIFECYCLESTAGE';

const OBSERVE_LOP = 'OBSERVE_LOP';

function commitRoot(commit, type, payload) {
  commit(type, payload, { root: true });
}

const initialState = {
  currentLifeCycleStage: {
    lifeCycleStageType: '',
    lifeCycleStageTypeName: '',
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
    state.currentLifeCycleStage = lifeCycleStage;
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
  constructor(router) {
    this.actions = {

      loadLifeCycle({ commit, state }, payload = {}) {
        commit(SET_MYSERVICE_LIFECYCLE, []);
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycle(payload.id,
          payload.sortOrder || state.listProperties.sorting,
          payload.paging || state.listProperties.paging,
          payload.lop || state.lop)
          .then((response) => {
            const lifeCycleResponse = new LifeCycleResponse(response);
            commit(SET_MYSERVICE_LIFECYCLE, lifeCycleResponse.lifeCycle);
            commit(RECEIVE_SORTING, lifeCycleResponse.sorting);
            commit(RECEIVE_PAGING, lifeCycleResponse.pagination);
          }).catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          }).finally(() => {
            commitRoot(commit, LOADING_OFF);
          });
      },

      loadLifeCycleStage({ commit }, { publicServiceId, lifeCycleStageId }) {
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycleStage(publicServiceId, lifeCycleStageId)
          .then(response => commit(
            SET_CURRENT_LIFECYCLESTAGE,
            new LifeCycleStageResponse(response).lifeCycleStage))
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          }).finally(() => {
            commitRoot(commit, LOADING_OFF);
          });
      },

      addStageToLifeCycle({ commit }, { id, data }) {
        commitRoot(commit, LOADING_ON);

        api
          .addStageToLifeCycle(new AddStageToLifeCycle(id, data))
          .then(result => {
            router.push({ name: 'my-service-life-cycle', params: { id: id, lop: result.data } });
            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      changePeriodForLifeCycleStage({ commit }, { id, lifeCycleStageId, data }) {
        commitRoot(commit, LOADING_ON);

        console.log('data', data);
        const request = new ChangePeriodForLifeCycleStage(id, lifeCycleStageId, data);
        console.log('request', request);

        return api.changePeriodOfLifeCycleStage(request)
          .then(result => {
            router.push({ name: 'my-service-life-cycle', params: { id: id, lop: result.data } });
            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      removeLifeCycleStage({ commit }, { id, lifeCycleStageId }) {
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
