import Router from 'vue-router';

import administrationRoutes from 'pages/administration/routes';
import systemRoutes from 'pages/system/routes';

import securityRoutes from 'pages/security/routes';

import allServicesRoutes from 'pages/all-services/routes';
import myServicesRoutes from 'pages/my-services/routes';

export default function () {
  return new Router({
    mode: 'history',
    base: '/',
    routes: [
      ...administrationRoutes,
      ...allServicesRoutes,
      ...myServicesRoutes,
      ...systemRoutes,
      ...securityRoutes,
    ],
  });
}
