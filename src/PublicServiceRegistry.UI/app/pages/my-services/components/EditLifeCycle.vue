<template>
  <form class="form col--10-12 col--1-1--s main-navigation__content" @submit.prevent="save">
    <div class="cta-title">
      <h2 class="h3 cta-title__title">Levensfase bewerken</h2>
    </div>

    <dv-form-row>
      <dv-label class="label" for="from">Geldig vanaf</dv-label>
      <dv-date-picker
        id="from"
        name="Geldig vanaf"
        :value="initialFrom"
        @change.native="updateChangedFrom"
        :validation="'required|date_format:DD.MM.YYYY' + (to ? '|before:' + to + ', true' : '')"
        :disabled="inputDisabled" />
      <dv-form-error id="Geldig vanaf"></dv-form-error>
    </dv-form-row>

    <dv-form-row>
      <dv-label class="label" for="to">Geldig tot</dv-label>
      <dv-date-picker
        id="to"
        name="Geldig tot"
        :min="from"
        :value="to"
        @change.native="updateChangedTo"
        :validation="'date_format:DD.MM.YYYY' + (from ? '|after:' + from + ', true' : '')"
        :disabled="inputDisabled" />
      <dv-form-error id="Geldig tot"></dv-form-error>
    </dv-form-row>

    <dv-form-row>
      <dv-button
        label="Toevoegen"
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
import DvDatePicker from 'components/form-elements/date-picker/DatePicker';
import DvSelect from 'components/form-elements/select/Select';
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
    DvDatePicker,
    DvSelect,
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
      lifeCycleStages: 'lifeCycleStages',
      lifeCycleStage: 'lifeCycleStage'
    }),
    localLifeCycleStage() {
      return this.lifeCycleStage(this.$route.params.localId);
    },
    initialFrom() {
      return this.localLifeCycleStage.from;
    },
    inputDisabled() {
      return this.isLoading;
    },
    buttonDisabled() {
      return this.isLoading || this.$validator.errors.any();
    },
    localLifeCyclesStages() {
      return this.lifeCycleStages.map((x) => {
        return {
          type: 'option',
          value: x.id,
          label: x.id,
          selected: x.id == this.selectedLifeCycleStage,
        };
      });
    },
  },
  methods: {
    updateChangedFrom(event){
      this.from = event.target.value;
    },
    updateChangedTo(event){
      this.to = event.target.value;
    },
    updateSelectedLifeCycleStage(event) {
      this.selectedLifeCycleStage = event.target.value;
    },
    save() {
      this.$validator.validateAll()
        .then(isSucces => {
          if(isSucces){
            this.$store.dispatch(
              "services/setPeriodForLifeCycle", {
                params: this.$router.currentRoute.params,
                data: {
                  levensloopfase: this.selectedLifeCycleStage,
                  vanaf: this.$formatDate(this.from),
                  tot: this.$formatDate(this.to),
                }
              });
          }
        })
        .catch(error => {
        });
    }
  },
  mounted() {
    this.$store.dispatch('services/loadLifeCycleStages');
  },
  data() {
    return {
      from: '',
      to: '',
      selectedLifeCycleStage: '',
    };
  },
};
</script>