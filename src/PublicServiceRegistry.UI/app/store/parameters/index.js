import api from 'services/dienstverleningen';

import alerts from 'store/alerts';

import {
  SET_ALERT,
  LOADING_OFF,
  LOADING_ON,
} from 'store/mutation-types';

const SET_LABELTYPES = 'SET_LABELTYPES';
const SET_LIFECYCLESTAGETYPES = 'SET_LIFECYCLESTAGES';

function commitRoot(commit, type, payload) {
  commit(type, payload, { root: true });
}

const initialState = {
  labelTypes: [],
  lifeCycleStageTypes: [],
};

// getters
const getters = {
  labelTypes: state => state.labelTypes.map(x => x.id),
  lifeCycleStageTypes: state => state.lifeCycleStageTypes,
};

const mutations = {
  [SET_LABELTYPES](state, labelTypes) {
    state.labelTypes = labelTypes;
  },
  [SET_LIFECYCLESTAGETYPES](state, lifeCycleStageTypes) {
    state.lifeCycleStageTypes = lifeCycleStageTypes;
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

      loadAlternativeLabelTypes({ commit }) {
        commitRoot(commit, LOADING_ON);

        return api.getLabelTypes()
          .then(labelTypes => commit(SET_LABELTYPES, labelTypes.data))
          .catch(error => commitRoot(commit, SET_ALERT, alerts.toAlert(error)))
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      loadLifeCycleStageTypes({ commit }) {
        commitRoot(commit, LOADING_ON);

        return api.getLifeCycleStageTypes()
          .then(lifeCycleStageTypes => commit(SET_LIFECYCLESTAGETYPES, lifeCycleStageTypes.data))
          .catch(error => commitRoot(commit, SET_ALERT, alerts.toAlert(error)))
          .finally(() => commitRoot(commit, LOADING_OFF));
      },
    };
  }
}
