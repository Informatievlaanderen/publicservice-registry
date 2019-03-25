import alertTypes from './alert-types';

export default {
  generalError: {
    title: 'Er is een fout opgetreden',
    content: 'Er is een algemene fout opgetreden.',
    type: alertTypes.error,
  },

  connectionError: {
    title: 'Er is een fout opgetreden',
    content: 'De server kon niet worden bereikt.',
    type: alertTypes.error,
  },

  createDomainError(detail) {
    return {
      title: 'Er is een fout opgetreden',
      content: detail,
      type: alertTypes.error,
    };
  },

  lopError: {
    title: 'Pas op',
    content: 'U werkt mogelijks met verouderde data.',
    type: alertTypes.warning,
  },

  serverError: {
    title: 'Er is een fout opgetreden',
    content: 'De server kan dit verzoek niet correct behandelen.',
    type: alertTypes.error,
  },

  empty: {
    title: '',
    content: '',
    type: '',
    visible: false,
  },

  unauthorized: {
    title: 'Geen toegang',
    content: 'U heeft geen toegang tot deze pagina.',
    type: alertTypes.error,
  },

  toAlert(error) {
    const { status, config = {}, response } = error;
    if (status === 200 && config.lop) {
      return this.lopError;
    }

    if (response) {
      const { data = {} } = response;
      if (response.status === 400 && data.detail) {
        return this.createDomainError(error.response.data.detail);
      }
      return this.serverError;
    } else if (error.request) {
      return this.connectionError;
    }

    console.error(error);
    return this.generalError;
  },
};
