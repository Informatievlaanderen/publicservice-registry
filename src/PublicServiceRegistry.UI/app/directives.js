import Vue from 'vue';

export default function prepareDirectives() {
  Vue.directive('focus', {
    // When the bound element is inserted into the DOM...
    inserted: (el) => {
      // Focus the element
      const children = el.getElementsByTagName('input');
      if (children.length > 0) {
        children.item(0).focus();
      }
    },
  });
}
