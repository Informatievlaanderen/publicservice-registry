import Vuex from 'vuex';

import { createLocalVue, mount } from '@vue/test-utils';

import UserInfo from 'components/partials/user-info/UserInfo';

function mockUserStore(getters) {
  return new Vuex.Store({
    modules: {
      user: {
        namespaced: true,
        getters,
      },
    },
  });
}

describe('UserInfo', () => {
  const localVue = createLocalVue();
  localVue.use(Vuex);

  test('is a Vue instance', () => {
    const store = mockUserStore({
      isLoggedIn: () => false,
    });

    const wrapper = mount(UserInfo, {
      localVue,
      store,
    });
    expect(wrapper.isVueInstance()).toBeTruthy();
  });

  test('shows \'Aanmelden\' when user is not logged in', () => {
    const store = mockUserStore({
      isLoggedIn: () => false,
    });

    const wrapper = mount(UserInfo, {
      localVue,
      store,
    });

    const header = wrapper.find('.functional-header__actions');
    expect(header.text()).toEqual('Aanmelden');
  });

  test('shows user description and \'Afmelden\' when user is not logged in', () => {
    const store = mockUserStore({
      isLoggedIn: () => true,
      userDescription: () => 'Metsu Koen',
    });

    const wrapper = mount(UserInfo, {
      localVue,
      store,
    });

    const header = wrapper.find('.functional-header__actions');
    expect(header.text()).toEqual('Metsu Koen Afmelden');
  });
});
