import Vue from 'vue';
import Vuex from 'vuex';

import alerts from '@/store/alerts';

import createStore, { mutations } from 'store/root';
import User from 'store/user';
import localStorage from 'store/user/localstorage';
import Services from 'store/services';
import routerFactory from '@/router';

const {
  LOADING_ON,
  LOADING_OFF,
  SET_ALERT,
  CLEAR_ALERT,
} = mutations;

describe('root store mutations', () => {
  Vue.use(Vuex);
  const store = createStore(new User(localStorage), new Services());
  routerFactory(Vue, store);

  it('LOADING_ON', () => {
    LOADING_ON(store.state);

    expect(store.state.isLoading).toBe(true);
  });

  it('LOADING_OFF', () => {
    LOADING_ON(store.state);

    LOADING_OFF(store.state);

    expect(store.state.isLoading).toBe(false);
  });

  it('SET_ALERT', () => {
    SET_ALERT(store.state, alerts.unauthorized);

    expect(store.state.alert).toEqual({
      ...alerts.unauthorized,
      visible: true,
    });
  });

  it('CLEAR_ALERT', () => {
    SET_ALERT(store.state, alerts.unauthorized);
    CLEAR_ALERT(store.state);

    expect(store.state.alert).toEqual({
      title: '',
      content: '',
      type: '',
      visible: false,
    });
  });
});
