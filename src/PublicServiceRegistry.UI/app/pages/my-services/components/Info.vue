<template>
  <form class="form col--10-12 col--1-1--s main-navigation__content" @submit.prevent="save">
    <div class="cta-title">
      <h2 class="h3 cta-title__title">Algemene informatie </h2>
    </div>

    <dv-form-row>
      <dv-label class="label" for="myServiceName">Naam</dv-label>
      <dv-input-field
        id="myServiceName"
        name="DienstNaam"
        :value="myServiceName"
        @input.native="updateName"
        :disabled="inputDisabled"
        v-focus
        validation="required" />
      <dv-form-error id="DienstNaam"></dv-form-error>
    </dv-form-row>

    <dv-form-row>
      <dv-label class="label" for="competentAuthority">Verantwoordelijke autoriteit</dv-label>
      <dv-input-field
        id="competentAuthority"
        name="Verantwoordelijke autoriteit"
        :value="myServiceCompetentAuthority"
        :placeholder="'OVO000001'"
        @input.native="updateCompetentAuthority"
        :disabled="inputDisabled"
        validation="required" />
      <dv-form-error id="Verantwoordelijke autoriteit"></dv-form-error>
    </dv-form-row>

    <dv-form-row>
      <dv-checkbox
        id="isSubsidy"
        name="IsSubsidie"
        :checked="myServiceIsSubsidy"
        label="Is subsidie?"
        @click.native="updateIsSubsidy"
        :disabled="inputDisabled" />
    </dv-form-row>

    <dv-form-row>
      <dv-button
        label="Opslaan"
        type="submit"
        :mod-is-loading="isLoading"
        :disabled="buttonDisabled" />
    </dv-form-row>
  </form>
</template>

<script>
import { mapGetters } from 'vuex';

import {
  UPDATE_MYSERVICE_NAME,
  UPDATE_MYSERVICE_COMPETENTAUTHORITY,
  UPDATE_MYSERVICE_ISSUBSIDY,
} from 'store/services';

import DvDataTable from 'components/data-table/DataTable';
import DvRouteButton from 'components/buttons/RouteButton';
import DvFormRow from 'components/form-elements/form-row/FormRow';
import DvLabel from 'components/form-elements/label/Label';
import DvButton from 'components/form-elements/button/Button';
import DvInputField from 'components/form-elements/input-field/InputField';
import DvCheckbox from 'components/form-elements/checkbox/Checkbox';
import DvFormError from 'components/form-elements/form-error/FormError';

export default {
  inject: ['$validator'],
  components: {
    DvRouteButton,
    DvDataTable,
    DvFormRow,
    DvLabel,
    DvInputField,
    DvButton,
    DvCheckbox,
    DvFormError,
  },
  computed: {
    ...mapGetters({
      inputDisabled: 'isLoading',
      isLoading: 'isLoading',
    }),
    ...mapGetters('services', {
      myServiceName: 'currentMyServiceName',
      myServiceCompetentAuthority: 'currentMyServiceCompetentAuthority',
      myServiceIsSubsidy: 'currentMyServiceIsSubsidy',
    }),
    inputDisabled() {
      return this.isLoading;
    },
    buttonDisabled() {
      return this.isLoading || this.$validator.errors.any();
    },
  },
  methods: {
    updateName(e) {
      this.$store.commit('services/UPDATE_MYSERVICE_NAME', e.target.value);
    },
    updateCompetentAuthority(e) {
      this.$store.commit('services/UPDATE_MYSERVICE_COMPETENTAUTHORITY', e.target.value);
    },
    updateIsSubsidy(e) {
      this.$store.commit('services/UPDATE_MYSERVICE_ISSUBSIDY', e.target.checked);
    },
    save() {
      this.$validator.validateAll()
        .then(isSucces => {
          if(isSucces){
            this.$store.dispatch("services/saveMyService");
          }
        })
        .catch(error => {
          // something went wrong (non-validation related).
        });
      // TODO: replace this by the code above in other forms.
      // if (!this.$validator.errors.any()) {
      //   this.$store.dispatch('saveMyService');
      // }
    },
  },
  watch: {

  },
  mounted() {
    this.$store.dispatch('services/loadMyService', this.$router.currentRoute.params).then(() => {
      this.$validator.validate();
    });
  },
  data() {
    return {
    };
  },
};
</script>
