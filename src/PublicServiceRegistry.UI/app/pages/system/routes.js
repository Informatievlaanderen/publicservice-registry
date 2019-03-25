import Overview from './Overview';
import Ipdc from './Ipdc';

export default [
  {
    path: '/systeem',
    name: 'system',
    component: Overview,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: '/systeem/ipdc',
    name: 'ipdc',
    component: Ipdc,
    meta: {
      requiresAuth: true,
    },
  },
];
