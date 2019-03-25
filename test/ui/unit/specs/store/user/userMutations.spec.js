import Vue from 'vue';
import Vuex from 'vuex';

import createStore from 'store/root';
import User from 'store/user';
import localStorage from 'store/user/localstorage';
import Services from 'store/services';
import routerFactory from '@/router';

Vue.use(Vuex);
const store = createStore(new User(localStorage), new Services());
routerFactory(Vue, store);

describe('user store mutations', () => {
  it('default state', () => {
    expect(store.state.isLoggedIn).toBeFalsy();
  });
});
