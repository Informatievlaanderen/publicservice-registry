import uuid from 'uuid';

import _ from 'lodash';

import axios from 'axios';

import api from 'services/dienstverleningen';

import alerts from 'store/alerts';
import {
  SET_ALERT,
  LOADING_OFF,
  LOADING_ON,
} from 'store/mutation-types';
import success from 'store/successes';
import csvExporter from 'services/csvExporter';

const RECEIVE_ALL_SERVICES = 'RECEIVE_ALL_SERVICES';
const RECEIVE_SORTING = 'RECEIVE_SORTING';
const RECEIVE_PAGING = 'RECEIVE_PAGING';

const UPDATE_ALLSERVICES_FILTER_NAME = 'UPDATE_ALLSERVICES_FILTER_NAME';
const UPDATE_ALLSERVICES_FILTER_DVRCODE = 'UPDATE_ALLSERVICES_FILTER_DVRCODE';
const UPDATE_ALLSERVICES_FILTER_COMPETENTAUTHORITY = 'UPDATE_ALLSERVICES_FILTER_COMPETENTAUTHORITY';

const UPDATE_MYSERVICE_NAME = 'UPDATE_MYSERVICE_NAME';
const UPDATE_MYSERVICE_COMPETENTAUTHORITY = 'UPDATE_MYSERVICE_COMPETENTAUTHORITY';
const UPDATE_MYSERVICE_ISSUBSIDY = 'UPDATE_MYSERVICE_ISSUBSIDY';

const SET_MYSERVICE = 'SET_MYSERVICE';
const RESET_NEWSERVICE = 'RESET_NEWSERVICE';

const SET_LABELTYPES = 'SET_LABELTYPES';
const SET_LIFECYCLESTAGES = 'SET_LIFECYCLESTAGES';

const SET_ALTERNATIVELABELS = 'SET_ALTERNATIVELABELS';
const SET_MYSERVICE_LIFECYCLE = 'SET_MYSERVICE_LIFECYCLE';

function commitRoot(commit, type, payload) {
  commit(type, payload, { root: true });
}

const initialState = {
  newService: {
    name: '',
  },
  currentMyService: {
    name: '',
    competentAuthority: '',
    isSubsidy: false,
  },
  servicesFilter: {
    name: '',
    dvrCode: '',
    competentAuthority: '',
  },
  services: [],
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
  labelTypes: [],
  lifeCycleStages: [],
  alternativeLabels: [],
  lifeCycle: [],
};

// getters
const getters = {
  newServiceName: state => state.newService.name,
  servicesFilter: state => state.servicesFilter,
  allServices: state => state.services || [],
  numberOfServices: state => (state.services || []).length,
  currentMyService: state => state.currentMyService,
  currentMyServiceId: state => state.currentMyService.id,
  currentMyServiceName: state => state.currentMyService.name,
  currentMyServiceCompetentAuthority: state => state.currentMyService.competentAuthority,
  currentMyServiceIsSubsidy: state => state.currentMyService.isSubsidy,
  sortColumn: (state) => {
    const sorting = state.listProperties.sorting;
    return {
      sortField: sorting.field,
      direction: sorting.direction,
    };
  },
  paging: state => state.listProperties.paging,
  labelTypes: state => state.labelTypes.map(x => x.id),
  lifeCycleStages: state => state.lifeCycleStages,
  alternativeLabels: state => _.reduce(state.alternativeLabels, (result, value) => {
    // eslint-disable-next-line no-param-reassign
    result = result || {};
    // eslint-disable-next-line no-param-reassign
    result[value.labelType] = value.labelValue;
    return result;
  }, {}),
  lifeCycle: state => state.lifeCycle || [],
  lifeCycleStage: state => localId => (state.lifeCycle || []).find(x => x.localId === localId),
  numberOfLifeCycleStages: state => (state.lifeCycle || []).length,
};

const mutations = {
  [RECEIVE_ALL_SERVICES](state, services) {
    state.services = services;
  },
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
  [UPDATE_MYSERVICE_NAME](state, name) {
    state.currentMyService.name = `${name}`;
  },
  [UPDATE_ALLSERVICES_FILTER_NAME](state, value) {
    state.servicesFilter.name = `${value}`;
  },
  [UPDATE_ALLSERVICES_FILTER_DVRCODE](state, value) {
    state.servicesFilter.dvrCode = `${value}`;
  },
  [UPDATE_ALLSERVICES_FILTER_COMPETENTAUTHORITY](state, value) {
    state.servicesFilter.competentAuthority = `${value}`;
  },
  [UPDATE_MYSERVICE_COMPETENTAUTHORITY](state, value) {
    state.currentMyService.competentAuthority = `${value}`;
  },
  [UPDATE_MYSERVICE_ISSUBSIDY](state, isSubsidy) {
    state.currentMyService.isSubsidy = !!isSubsidy;
  },
  [RESET_NEWSERVICE](state) {
    state.newService.name = '';
  },
  [SET_MYSERVICE](state, myService) {
    state.currentMyService.id = myService.id;
    state.currentMyService.name = myService.naam;
    state.currentMyService.competentAuthority = myService.verantwoordelijkeAutoriteitCode;
    state.currentMyService.isSubsidy = myService.exportNaarOrafin;
  },
  [SET_LABELTYPES](state, labelTypes) {
    state.labelTypes = labelTypes;
  },
  [SET_LIFECYCLESTAGES](state, lifeCycleStages) {
    state.lifeCycleStages = lifeCycleStages;
  },
  [SET_ALTERNATIVELABELS](state, alternativeLabels) {
    state.alternativeLabels = alternativeLabels;
  },
  [SET_MYSERVICE_LIFECYCLE](state, lifeCycle) {
    state.lifeCycle = lifeCycle;
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
      save({ commit }, payload) {
        commitRoot(commit, LOADING_ON);

        const myService = {
          id: uuid.v4(),
          naam: payload.name,
        };

        api
          .createMyService(myService)
          .then((response) => {
            commitRoot(commit, LOADING_OFF);
            commit(RESET_NEWSERVICE);
            const dvrCodeRegex = /\/v[0-9]\/dienstverleningen\/(.*)/;
            const dvrCode = dvrCodeRegex.exec(response.headers.location)[1];
            router.push({ name: 'my-service-info', params: { id: dvrCode, lop: response.data } });
            commitRoot(commit, SET_ALERT, success.dienstverleningAangemaakt);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
            commitRoot(commit, LOADING_OFF);
          });
      },

      loadMyService({ commit }, { id, lop }) {
        commitRoot(commit, LOADING_ON);

        return api.getMyService(id, lop)
          .then((result) => {
            commit(SET_MYSERVICE, result.data);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => {
            commitRoot(commit, LOADING_OFF);
          });
      },

      loadAlternativeLabels({ commit }, { id }) {
        commitRoot(commit, LOADING_ON);

        return axios.all([
          api.getLabelTypes(),
          api.getAlternativeLabels(id),
        ]).then(axios.spread((labelTypes, alternativeLabels) => {
          commit(SET_LABELTYPES, labelTypes.data);
          commit(SET_ALTERNATIVELABELS, alternativeLabels.data);
        })).catch((error) => {
          commitRoot(commit, SET_ALERT, alerts.toAlert(error));
        }).finally(() => {
          commitRoot(commit, LOADING_OFF);
        });
      },

      saveAlternativeLabels({ commit }, { params: { id }, labels }) {
        commitRoot(commit, LOADING_ON);

        api
          .updateAlternativeLabels(id, labels)
          .then(() => {
            // todo: in the new ui, we will go to a 'read' page which expects the lop to have been reached.
            // router.push({ name: 'my-service', params: { id: myService.id, lop: response.data } });

            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      loadLifeCycle({ commit }, payload = {}) {
        commit(RECEIVE_ALL_SERVICES, {});
        commit(RECEIVE_SORTING, {});
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycle(payload.routerParams.id)
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

      loadLifeCycleStages({ commit }) {
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycleStages()
          .then(lifeCycleStages => commit(SET_LIFECYCLESTAGES, lifeCycleStages.data))
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          }).finally(() => {
            commitRoot(commit, LOADING_OFF);
          });
      },

      setPeriodForLifeCycle({ commit }, { params: { id }, data }) {
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

      saveMyService({ commit, state }) {
        commitRoot(commit, LOADING_ON);

        const myService = {
          id: state.currentMyService.id,
          naam: state.currentMyService.name,
          bevoegdeAutoriteitOvoNummer: state.currentMyService.competentAuthority,
          isSubsidie: state.currentMyService.isSubsidy,
        };

        api
          .updateMyService(myService)
          .then(() => {
            commit(RESET_NEWSERVICE);

            // todo: in the new ui, we will go to a 'read' page which expects the lop to have been reached.
            // router.push({ name: 'my-service', params: { id: myService.id, lop: response.data } });

            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      removeService({ commit }, { params: { id }, reasonForRemoval }) {
        commitRoot(commit, LOADING_ON);

        api
          .removeService(id, reasonForRemoval)
          .then((response) => {
            router.push({ name: 'all-services', params: { lop: response.data } });
            commitRoot(commit, SET_ALERT, success.dienstverleningAangepast);
          })
          .catch(error => commitRoot(commit, SET_ALERT, alerts.toAlert(error)))
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      getAllServices({ commit, state }, payload = {}) {
        commit(RECEIVE_ALL_SERVICES, {});
        commit(RECEIVE_SORTING, {});
        commitRoot(commit, LOADING_ON);

        api.getAllServices(
          state.servicesFilter,
          payload.sortOrder,
          payload.paging || state.listProperties.paging,
          payload.routerParams)
          .then(({ data, headers }) => {
            commit(RECEIVE_ALL_SERVICES, data);
            commit(RECEIVE_SORTING, JSON.parse(headers['x-sorting'] || null));
            commit(RECEIVE_PAGING, JSON.parse(headers['x-pagination'] || null));
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      filterServices({ dispatch, commit }, payload) {
        commit(UPDATE_ALLSERVICES_FILTER_DVRCODE, payload.dvrCode);
        commit(UPDATE_ALLSERVICES_FILTER_NAME, payload.serviceName);
        commit(UPDATE_ALLSERVICES_FILTER_COMPETENTAUTHORITY, payload.competentAuthority);
        dispatch('getAllServices');
      },

      exportServicesAsCsv({ commit, state }) {
        commit(RECEIVE_ALL_SERVICES, {});
        commit(RECEIVE_SORTING, {});
        commitRoot(commit, LOADING_ON);

        api.getAllServicesAsCsv(state.servicesFilter)
          .then((r) => {
            csvExporter.export(r.data);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },
    };
  }
}