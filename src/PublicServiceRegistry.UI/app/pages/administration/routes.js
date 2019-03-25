import Overview from './Overview';

export default [
  {
    path: '/parameters',
    name: 'administration',
    component: Overview,
    meta: {
      requiresAuth: true,
    },
  },
];
