import Vue from 'vue';
import Vuex from 'vuex';
import VueRouter from 'vue-router';
import axios from 'axios';
// import { sync } from 'vuex-router-sync';
/* eslint-disable-next-line */
import es6shim from 'es6-shim';
import { shim } from 'promise.prototype.finally';
import VeeValidate from 'vee-validate';
import moment from 'vue-moment';

import createStore from 'store/root';
import User from 'store/user';
import localStorage from 'store/user/localstorage';
import Services from 'store/services';
import ServiceLifeCycle from 'store/services/life-cycle';
import Ipdc from 'store/ipdc';
import Parameters from 'store/parameters';

import clearAlertOnNavigate from 'store/clearAlertOnNavigate';

import App from './App';
import routerFactory from './router';
import validatorConfig from './validation';
import registerDirectives from './directives';
import registerDefaultComponents from './defaultComponents';

import OidcClient from './oidc';

import formatDatePlugin from './plugins/formatDate';

Vue.config.productionTip = false;

shim();

axios.defaults.baseURL =
  window.dienstverleningApiEndpoint ||
  'https://api.dienstverlening-test.basisregisters.vlaanderen:8003/';

const oidcClient = new OidcClient(axios);

// axios.defaults.headers.common['Authorization'] = AUTH_TOKEN;

axios.defaults.headers.common.Accept = 'application/json';
axios.defaults.headers.common.Authorization = `Bearer ${window.localStorage.token}`;
axios.defaults.headers.common['Content-Type'] = 'application/json';
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.withCredentials = true;

// retries by https://github.com/axios/axios/issues/164#issuecomment-327837467
axios.interceptors.response.use(
  (response) => {
    const expectedLop = response.config.lop;
    if (!expectedLop) return response;

    const config = response.config || {};
    config.retry = config.retry || 8;
    config.retryDelay = response.config.retryDelay || 700;

    const actualLop = response.headers['x-lop'];
    if (!actualLop) return response;

    // this is ok
    if (actualLop >= expectedLop) return response;

    // Set the variable for keeping track of the retry count
    config.myRetryCount = config.myRetryCount || 0;

    // Check if we've maxed out the total number of retries
    // Reject with the error
    if (config.myRetryCount >= config.retry) return Promise.reject(response);

    // Increase the retry count
    config.myRetryCount += 1;

    const backoff = new Promise((resolve) => {
      setTimeout(() => {
        resolve();
      }, config.retryDelay || 1);
    });

    return backoff.then(() => axios(config));
  },
  (err) => {
    if (err.response.status === 401 &&
      err.request.responseURL !== `${axios.defaults.baseURL}v2/security`) {
      // const currentUrl = window.location.toString();
      // const authUrl = `${axios.defaults.baseURL}v2/security/signin?returnUrl=${currentUrl}`;
      // window.location.href = authUrl;
      oidcClient.signIn();
    }

    const response = err.response;
    const expectedLop = response.config.lop;

    if (!expectedLop) return Promise.reject(err);

    const config = response.config || {};
    config.retry = config.retry || 8;
    config.retryDelay = response.config.retryDelay || 700;

    const actualLop = err.response.headers['x-lop'];

    if (!actualLop) return Promise.reject(err);

    if (actualLop >= expectedLop) {
      // this is ok

      return Promise.reject(err);
    }

    // Set the variable for keeping track of the retry count
    config.myRetryCount = config.myRetryCount || 0;

    // Check if we've maxed out the total number of retries
    if (config.myRetryCount >= config.retry) {
      // Reject with the error
      return Promise.reject(err);
    }

    // Increase the retry count
    config.myRetryCount += 1;

    const backoff = new Promise((resolve) => {
      setTimeout(() => {
        resolve();
      }, config.retryDelay || 1);
    });

    return backoff.then(() => axios(config));
  });


// Define the components name.

registerDefaultComponents();
registerDirectives();

Vue.use(VeeValidate, validatorConfig());
Vue.use(Vuex);
Vue.use(VueRouter);
Vue.use(moment);
Vue.use(formatDatePlugin);

const router = routerFactory();
const store = createStore(
  new User(localStorage, router, oidcClient),
  new Services(router, new ServiceLifeCycle(router)),
  new Ipdc(),
  new Parameters());

// sync(store, router);
clearAlertOnNavigate(store, router);

router.afterEach(() => { });

store
  .dispatch('user/loadUserInfo')
  .then(() => {
    /* eslint-disable no-new */
    new Vue({
      el: '#app',
      router,
      store,
      components: { App },
      template: '<App/>',
    });
  });
