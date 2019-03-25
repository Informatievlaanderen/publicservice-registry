import Callback from './Callback';

export default [
  { path: '/oic', name: 'openidconnect', component: Callback, props: route => ({ code: route.query.code }) },
];
