import axios from 'axios';
import jwtDecode from 'jwt-decode';

import api from 'services/dienstverleningen';

import {
  SET_ALERT,
} from 'store/mutation-types';
import alerts from 'store/alerts';

const SET_AUTHENTICATED = 'SET_AUTHENTICATED';
const SET_UNAUTHENTICATED = 'SET_UNAUTHENTICATED';

function commitRoot(commit, type, payload) {
  commit(type, payload, { root: true });
}

function prepareActions(localStorageWrapper, router, oidcClient) {
  return {
    loadUserInfo({ commit }) {
      try {
        const token = localStorageWrapper.getToken();
        if (token) {
          const decoded = jwtDecode(token);
          commit(SET_AUTHENTICATED, {
            firstName: decoded.given_name,
            name: decoded.family_name,
          });
        } else {
          commit(SET_UNAUTHENTICATED);
        }
      } catch (exception) {
        console.log(exception);
      }
    },
    exchangeToken({ commit }, code) {
      axios
        .get(`/v2/security/exchange?code=${code}`)
        .then((response) => {
          // do these belong in the mutations instead?
          localStorageWrapper.setToken(response.data);
          axios.defaults.headers.common.Authorization = `Bearer ${localStorageWrapper.getToken()}`;

          // duplicate code below, see loadUserInfo
          api
            .loadUserInfo()
            .then((userResponse) => {
              commit(SET_AUTHENTICATED, userResponse.data);
              router.push('/');
            })
            .catch((error) => {
              commit(SET_UNAUTHENTICATED, error);
              commitRoot(commit, SET_ALERT, alerts.toAlert(error));
            });
        });

      // because we use oidc-client to create the sign in request,
      // but not oidc-client to process the result,
      // the oidc-client's state (stored in storage) is not
      // properly removed. Remove it manually here.
      Object.keys(localStorageWrapper)
        .filter(item => item.startsWith('oidc.'))
        .forEach(item => localStorageWrapper.removeItem(item));
    },
    logIn() {
      oidcClient.signIn();
    },
    logOut({ commit }) {
      localStorageWrapper.removeItem('token');
      axios.defaults.headers.common.Authorization = '';
      commit(SET_UNAUTHENTICATED);
      oidcClient.signOut();
    },
  };
}

const initialState = {
  isLoggedIn: false,
  name: '',
  firstName: '',
  roles: [],
};

export function getDefaultState() {
  return { ...initialState };
}

export default function prepareStore(localStorageWrapper, router, oidcClient) {
  return {
    namespaced: true,
    state: initialState,
    getters: {
      isLoggedIn: state => state.isLoggedIn,
      userDescription: state => `${state.name} ${state.firstName}`,
    },
    mutations: {
      [SET_AUTHENTICATED](state, {
        name,
        firstName,
      }) {
        state.isLoggedIn = true;
        state.name = name;
        state.firstName = firstName;
      },
      [SET_UNAUTHENTICATED](state) {
        state.isLoggedIn = false;
      },
    },
    actions: prepareActions(localStorageWrapper, router, oidcClient),
  };
}
