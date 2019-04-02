import Vuex from 'vuex';

import alerts from 'store/alerts';

import getters from './getters';

import {
  SET_ALERT,
  CLEAR_ALERT,
  LOADING_OFF,
  LOADING_ON,
} from '../mutation-types';

export const mutations = {
  [LOADING_ON](state) {
    state.isLoading = true;
  },
  [LOADING_OFF](state) {
    state.isLoading = false;
  },
  [CLEAR_ALERT](state) {
    state.alert = alerts.empty;
  },
  [SET_ALERT](state, alert) {
    state.alert = {
      title: alert.title,
      content: alert.content,
      type: alert.type,
      visible: true,
    };
  },
};

const state = {
  isLoading: false,
  alert: {
    title: '',
    content: '',
    type: '',
    visible: false,
  },
};

export function getDefaultState() { return { ...state }; }

export default function createStore(userStore, servicesStore, ipdcStore, parametersStore) {
  return new Vuex.Store({
    modules: {
      user: userStore,
      services: servicesStore,
      ipdc: ipdcStore,
      parameters: parametersStore,
    },
    state,
    getters,
    mutations,
  });
}
