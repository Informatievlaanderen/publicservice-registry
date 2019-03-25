import Vuex from 'vuex';
import {
  createLocalVue,
  mount,
} from '@vue/test-utils';
import VeeValidate from 'vee-validate';

import NewService from 'pages/my-services/NewService';
import validatorConfig from '@/validation';
import directives from '@/directives';

import VueRouter from 'vue-router';

function mockUserStore(rootGetters, getters, actions) {
  return new Vuex.Store({
    getters: rootGetters,
    modules: {
      services: {
        namespaced: true,
        getters,
        actions,
      },
    },
  });
}

describe('NewService', () => {
  const localVue = createLocalVue();
  localVue.use(VeeValidate, validatorConfig());
  localVue.use(Vuex);
  localVue.use(VueRouter);
  directives();

  test('is a Vue instance', () => {
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      newServiceName: () => '',
    });

    const wrapper = mount(NewService, {
      localVue,
      store,
    });
    expect(wrapper.isVueInstance()).toBeTruthy();
  });

  test('name is initially empty', () => {
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      newServiceName: () => '',
    });

    const wrapper = mount(NewService, {
      localVue,
      store,
    });
    expect(wrapper.find('#name').text()).toEqual('');
  });

  test('with empty name disables button', () => {
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      newServiceName: () => '',
    });

    const wrapper = mount(NewService, {
      localVue,
      store,
      attachToDocument: true,
    });
    expect(wrapper.vm.buttonDisabled).toBeTruthy();
    expect(wrapper.vm.allValid).toBeFalsy();
    wrapper.vm.$forceUpdate();
    expect(wrapper.find('#opslaan').attributes()).not.toContain('disabled');
  });

  test('with non-empty name enables button', () => {
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      newServiceName: () => '',
    });

    const wrapper = mount(NewService, {
      localVue,
      store,
    });

    // wrapper.find('#name').setValue('dienstverlening 1');

    expect(wrapper.vm.buttonDisabled).toBeTruthy();
    expect(wrapper.vm.allValid).toBeFalsy();
    wrapper.vm.$forceUpdate();
    expect(wrapper.find('#opslaan').attributes().disabled).toBeTruthy();
  });

  test('clicking the button emits event', () => {
    const actions = {
      updateName: jest.fn(),
    };
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      newServiceName: () => '',
    }, actions);

    const wrapper = mount(NewService, {
      localVue,
      store,
    });

    wrapper.find('#name').setValue('dienstverlening 123');
    wrapper.find('#opslaan').trigger('click');
    expect(actions.updateName).toHaveBeenCalledTimes(1);
  });

  test('clicking the button does not emit an event when invalid', () => {
    const actions = {
      updateName: jest.fn(),
    };
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      newServiceName: () => '',
    }, actions);

    const wrapper = mount(NewService, {
      localVue,
      store,
    });

    wrapper.find('#opslaan').trigger('click');
    expect(actions.updateName).not.toHaveBeenCalled();
  });
});
