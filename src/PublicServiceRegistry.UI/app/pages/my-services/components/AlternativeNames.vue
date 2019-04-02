<template>
  <form class="form col--10-12 col--1-1--s main-navigation__content" @submit.prevent="save">
    <div class="cta-title">
      <h2 class="h3 cta-title__title">Alternatieve benamingen </h2>
    </div>

    <dv-form-row v-for="labelType in labelTypes" :key="labelType">
      <dv-label class="label" for="myServiceName">{{labelType}}</dv-label>
      <dv-input-field
        :id="labelType"
        :name="labelType"
        :value="valueFor(labelType)"
        @input.native="update(labelType, $event)"
        :disabled="inputDisabled"
        v-focus />
      <dv-form-error :id="labelType"></dv-form-error>
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
    ...mapGetters('parameters', {
      labelTypes: 'labelTypes',
    }),
    ...mapGetters('services', {
      myServiceName: 'currentMyServiceName',
      myServiceCompetentAuthority: 'currentMyServiceCompetentAuthority',
      myServiceIsSubsidy: 'currentMyServiceIsSubsidy',
      alternativeLabels: 'alternativeLabels',
    }),
    inputDisabled() {
      return this.isLoading;
    },
    buttonDisabled() {
      return this.isLoading || this.$validator.errors.any();
    },
  },
  methods: {
    update(e, event) {
      this.labels[e] = event.target.value;
    },
    valueFor(labelType) {
      return this.labels[labelType];
    },
    save() {
      this.$validator.validateAll()
        .then(isSucces => {
          if(isSucces){
            this.$store.dispatch(
              "services/saveAlternativeLabels", {
                params: this.$router.currentRoute.params,
                labels: this.labels
              });
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
  mounted() {
    this.$store.dispatch('services/loadAlternativeLabels', this.$router.currentRoute.params).then(() => {
      this.$validator.validate();
      this.labels = this.alternativeLabels;
    });
  },
  data() {
    return {
      labels: {},
    };
  },
};
</script>
