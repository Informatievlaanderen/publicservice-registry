<template>
  <dv-functional-header-actions v-if="isLoggedIn">
    <dv-functional-header-action :title="userDescription" />
    <dv-functional-header-action title="Afmelden" :on="logoutClicked" />
  </dv-functional-header-actions>
  <dv-functional-header-actions v-else>
    <dv-functional-header-action title="Aanmelden" :on="loginClicked" />
  </dv-functional-header-actions>
</template>

<script>

import DvLayout from '../../frame/layout/Layout';
import DvFunctionalHeaderAction from '../functional-header/FunctionalHeaderAction';
import DvFunctionalHeaderActions from '../functional-header/FunctionalHeaderActions';

import { mapGetters } from 'vuex';

export default {
  name: 'user-info',
  components: {
    DvLayout,
    DvFunctionalHeaderAction,
    DvFunctionalHeaderActions,
  },
  computed: {
    ...mapGetters('user', {
      isLoggedIn: 'isLoggedIn',
      userDescription: 'userDescription',
    }),
  },
  methods: {
    loginClicked() {
      this.$store.dispatch('user/logIn');
    },
    logoutClicked() {
      this.$store.dispatch('user/logOut');
    },
  },
  props: {
    modHasActions: {
      default: false,
      type: Boolean,
    },
  },
  data() {
    return {
      classes: {
        'functional-header': true,
        'functional-header--has-actions': this.modHasActions,
      },
    };
  },
};
</script>

<style scoped>
  .functional-header {
    margin-bottom: 0;
  }

  @media screen and (max-width: 767px) {
    .functional-header--has-actions:before, .functional-header__actions {
      display: block;
    }
  }
</style>
