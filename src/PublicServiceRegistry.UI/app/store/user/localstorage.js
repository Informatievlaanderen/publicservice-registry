export default {
  getToken() {
    return window.localStorage.token;
  },

  setToken(value) {
    window.localStorage.token = value;
  },

  removeItem(item) {
    window.localStorage.removeItem(item);
  },
};
