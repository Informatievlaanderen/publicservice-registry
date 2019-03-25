import NewService from './NewService';
import Service from './Service';
import ServiceView from './ServiceView';
import ServiceInfo from './components/Info';
import ServiceAlternativeNames from './components/AlternativeNames';
import ServiceLifeCycle from './components/LifeCycle';
import ServiceNewLifeCycle from './components/NewLifeCycle';
import ServiceEditLifeCycle from './components/EditLifeCycle';
import ServiceAdvanced from './components/Advanced';

export default [
  {
    path: '/mijn-dienstverleningen/nieuw',
    name: 'new-service',
    component: NewService,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: '/mijn-dienstverleningen/:id',
    name: 'my-service-view',
    component: ServiceView,
    meta: {
      requiresAuth: false,
    },
  },
  {
    path: '/mijn-dienstverleningen/:id/edit',
    name: 'my-service',
    redirect: '/mijn-dienstverleningen/:id/edit/info',
    component: Service,
    meta: {
      requiresAuth: true,
    },
    children: [
      {
        path: 'info',
        name: 'my-service-info',
        props: true,
        component: ServiceInfo,
      },
      {
        path: 'benamingen',
        name: 'my-service-alternative-names',
        component: ServiceAlternativeNames,
      },
      {
        path: 'levensloop',
        name: 'my-service-life-cycle',
        component: ServiceLifeCycle,
      },
      {
        path: 'levensloop/new',
        name: 'my-service-new-life-cycle',
        component: ServiceNewLifeCycle,
      },
      {
        path: 'levensloop/edit/:localId',
        name: 'my-service-edit-life-cycle',
        component: ServiceEditLifeCycle,
      },
      {
        path: 'administratie',
        name: 'my-service-advanced',
        component: ServiceAdvanced,
      },
    ],
  },
];
