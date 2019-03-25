<template>
  <div class="cta-title">
    <h1 class="h2 cta-title__title">Even geduld aub...</h1>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex';
import axios from 'axios';

export default {
  props: ['code'],
  mounted() {
    this.$store.dispatch('user/exchangeToken', this.code);
  },
};
</script>
