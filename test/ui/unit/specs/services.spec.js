import Vuex from 'vuex';
import {
  createLocalVue,
  mount,
} from '@vue/test-utils';
import VeeValidate from 'vee-validate';

import Overview from 'pages/my-services/Overview';
import validatorConfig from '@/validation';
import registerDirectives from '@/directives';
import registerDefaultComponents from '@/defaultComponents';
import routerFactory from '@/router';
import VueRouter from 'vue-router';

function mockUserStore(rootGetters, getters, actions) {
  return new Vuex.Store({
    getters: rootGetters,
    mutations: {
      CLEAR_ALERT: () => { },
    },
    modules: {
      services: {
        namespaced: true,
        getters,
        actions,
      },
    },
  });
}

describe('MyServices Overview', () => {
  const localVue = createLocalVue();
  localVue.use(VeeValidate, validatorConfig());
  localVue.use(Vuex);

  registerDefaultComponents();
  registerDirectives();
  localVue.use(VueRouter);
  const router = routerFactory(mockUserStore());

  test('is a Vue instance', () => {
    const actions = {
      getAllServices: jest.fn().mockReturnValue([]),
    };
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      allServices: () => [],
      sortColumn: () => ({ sortField: '', direction: '' }),
      paging: () => ({
        totalItems: 0,
        itemsPerPage: 10,
        currentPage: 1,
        totalPages: 1,
      }),
    },
    actions);

    const wrapper = mount(Overview, {
      localVue,
      store,
      router,
    });
    expect(wrapper.isVueInstance()).toBeTruthy();
  });

  test('tells me when no data available', () => {
    const actions = {
      getAllServices: jest.fn().mockReturnValue([]),
    };
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      allServices: () => [],
      sortColumn: () => ({ sortField: '', direction: '' }),
      paging: () => ({
        totalItems: 0,
        itemsPerPage: 10,
        currentPage: 1,
        totalPages: 1,
      }),
    },
    actions);

    const wrapper = mount(Overview, {
      localVue,
      store,
      router,
    });

    expect(wrapper.find('.data-table-empty-result').text()).toEqual('Geen data beschikbaar...');
  });

  test('shows available data', () => {
    const actions = {
      getAllServices: jest.fn(),
    };
    const store = mockUserStore({
      isLoading: () => false,
    }, {
      allServices: () => [],
      sortColumn: () => ({ sortField: '', direction: '' }),
      paging: () => ({
        totalItems: 0,
        itemsPerPage: 10,
        currentPage: 1,
        totalPages: 1,
      }),
    },
    actions);

    const wrapper = mount(Overview, {
      localVue,
      store,
      router,
    });

    wrapper.vm.$refs.allServicesTable.setData([{ id: 1, name: 'test 1' }, { id: 2, name: 'test 2' }]);
    wrapper.vm.$nextTick(() => {
      expect(wrapper.findAll('.data-table-body>tr').length).toBe(2);
    });
  });
});
