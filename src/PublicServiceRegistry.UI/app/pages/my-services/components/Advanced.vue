<template>
  <form class="form col--10-12 col--1-1--s main-navigation__content" @submit.prevent="remove">
    <div style="border:2px solid #db3434;padding:20px;">
      <div class="cta-title">
        <h2 class="h3 cta-title__title">Verwijderen dienstverlening </h2>
      </div>

      <dv-form-row>
        <p>Deze actie <span style="font-weight:bold;">verwijdert</span> de dienstverlening <span style="font-weight:bold;">permanent</span> uit het register.</p>
        <p>Hierdoor zullen ook afnemers de dienstverlening niet meer kunnen vinden via de api.</p>
      </dv-form-row>

      <dv-form-row>
        <dv-label class="label" for="reasonForRemoval">Reden voor verwijdering</dv-label>
          <dv-input-field
            id="reasonForRemoval"
            name="Reden voor verwijdering"
            :value="reasonForRemoval"
            @input.native="updateReasonForRemoval"
            :disabled="inputDisabled"
            validation="required"
            v-focus />
          <dv-form-error id="Reden voor verwijdering"></dv-form-error>
      </dv-form-row>

      <dv-form-row>
        <dv-label class="label" for="nameOfService">Om de verwijdering te bevestigen, gelieve de naam van de dienstverlening te herhalen</dv-label>
        <dv-input-field
          id="nameOfService"
          name="Naam van de dienstverlening"
          :value="serviceToRemove"
          @input.native="updateNameOfService"
          :disabled="inputDisabled"
          :validation="'required|is:'+ this.myServiceName" />
        <dv-form-error id="Naam van de dienstverlening"></dv-form-error>
        <input type="hidden" name="serviceName" id="serviceName" :value="myServiceName" />
      </dv-form-row>

      <dv-form-row>
        <dv-button
          label="Verwijderen"
          type="submit"
          :mod-is-loading="isLoading"
          :mod-is-warning="true"
          :disabled="buttonDisabled" />
      </dv-form-row>
    </div>
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
    }),
    inputDisabled() {
      return this.isLoading;
    },
    buttonDisabled() {
      return this.isLoading || this.$validator.errors.any() || this.serviceToRemove !== this.myServiceName;
    },
  },
  methods: {
    updateReasonForRemoval(e) {
      this.reasonForRemoval = e.target.value;
    },
    updateNameOfService(e) {
      this.serviceToRemove = e.target.value;
    },
    remove() {
      this.$validator.validateAll()
        .then(isSucces => {
          if(isSucces){
            this.$store.dispatch(
              "services/removeService", {
                params: this.$router.currentRoute.params,
                reasonForRemoval: this.reasonForRemoval,
              });
          }
        })
        .catch(error => {
          // something went wrong (non-validation related).
        });
    },
  },
  mounted() {
    this.$store.dispatch('services/loadAlternativeLabels', this.$router.currentRoute.params).then(() => {
    });
  },
  data() {
    return {
      serviceToRemove: '',
      reasonForRemoval: '',
    };
  },
};
</script>
