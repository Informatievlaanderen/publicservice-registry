import api from 'services/dienstverleningen';

import alerts from 'store/alerts';
import {
  SET_ALERT,
  LOADING_OFF,
  LOADING_ON,
} from 'store/mutation-types';

function commitRoot(commit, type, payload) {
  commit(type, payload, { root: true });
}

const RECEIVE_CHANGED_PRODUCTS = 'RECEIVE_CHANGED_PRODUCTS';
const RECEIVE_CHANGED_SINCE = 'RECEIVE_CHANGED_SINCE';
const RECEIVE_PRODUCT = 'RECEIVE_PRODUCT';

const initialState = {
  products: [],
  product: {},
  changedSince: '',
};

// getters
const getters = {
  changedProducts: state => state.products || [],
  changedSince: state => state.changedSince,
  product: state => state.product,
};

const mutations = {
  [RECEIVE_CHANGED_PRODUCTS](state, products) {
    state.products = products;
  },
  [RECEIVE_CHANGED_SINCE](state, changedSince) {
    state.changedSince = changedSince;
  },
  [RECEIVE_PRODUCT](state, product) {
    state.product = product;
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
      updateChangedSince({ commit }, payload) {
        commit(RECEIVE_CHANGED_SINCE, payload);
      },

      getChangedProducts({ commit }, changedSince) {
        commit(RECEIVE_CHANGED_PRODUCTS, {});

        commitRoot(commit, LOADING_ON);

        api.getAllIpdcChanges(changedSince)
          .then(({ data }) => {
            commit(RECEIVE_CHANGED_PRODUCTS, data);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },

      getProduct({ commit }, productId) {
        commit(RECEIVE_PRODUCT, {});

        commitRoot(commit, LOADING_ON);

        api.getIpdcProduct(productId)
          .then(({ data }) => {
            commit(RECEIVE_PRODUCT, data);
          })
          .catch((error) => {
            commitRoot(commit, SET_ALERT, alerts.toAlert(error));
          })
          .finally(() => commitRoot(commit, LOADING_OFF));
      },
    };
  }
}
