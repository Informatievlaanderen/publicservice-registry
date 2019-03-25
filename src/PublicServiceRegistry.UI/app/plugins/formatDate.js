export default {
  install(Vue) {
    if (!Vue.moment) {
      // eslint-disable-next-line no-console
      console.error('Vue.moment is required by the formatDate plugin, but was not found. Please install vue-moment correctly.');
    }

    // eslint-disable-next-line no-param-reassign
    Vue.prototype.$formatDate = (date) => {
      if (!date || date === '') {
        return '';
      }

      return Vue.moment(date, 'DD.MM.YYYY').format('YYYY-MM-DD');
    };
  },
};
