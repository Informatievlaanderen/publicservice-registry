import Vue from 'vue';

import DvLayout from 'components/frame/layout/Layout';
import DvGrid from 'components/frame/grid/Grid';
import DvColumn from 'components/frame/column/Column';

export default function registerDefaultComponents() {
  const components = {
    DvLayout,
    DvGrid,
    DvColumn,
  };

  // Iterate through them and add them to
  // the global Vue scope.
  Object.keys(components).forEach(key => Vue.component(key, components[key]));
}
