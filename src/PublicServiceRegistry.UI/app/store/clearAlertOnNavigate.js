import alerts from 'store/alerts';

import {
  SET_ALERT,
  CLEAR_ALERT,
} from './mutation-types';


export default function (store, router) {
  router.beforeEach((to, _from, next) => {
    store.commit(CLEAR_ALERT);

    const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
    if (requiresAuth && !store.getters['user/isLoggedIn']) {
      store.commit(SET_ALERT, alerts.unauthorized);
    } else {
      next();
    }
  });
}
