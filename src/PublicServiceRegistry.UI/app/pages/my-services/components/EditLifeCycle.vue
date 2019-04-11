<template>
  <form class="form col--10-12 col--1-1--s main-navigation__content" @submit.prevent="save">
    <div class="cta-title">
      <h2 class="h3 cta-title__title">Levensfase bewerken</h2>
    </div>

    <dv-form-row>
      <dv-label class="label" for="from">Levensfase</dv-label>
      <dv-select
        id="lifeCycleStage"
        name="Levensfase"
        :options="lifeCycleStageTypes"
        :disabled="true" />
      <dv-form-error id="Levensfase"></dv-form-error>
    </dv-form-row>

    <dv-form-row>
      <dv-label class="label" for="from">Geldig vanaf</dv-label>
      <dv-date-picker
        id="from"
        name="Geldig vanaf"
        :value="from"
        @change.native="updateChangedFrom"
        :validation="'date_format:DD.MM.YYYY' + (to ? '|before:' + to + ', true' : '')"
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
        label="Bewerken"
        type="submit"
        :mod-is-loading="isLoading"
        :disabled="buttonDisabled" />
    </dv-form-row>
  </form>
</template>

<script>
import { mapGetters } from 'vuex';

import DvFormRow from 'components/form-elements/form-row/FormRow';
import DvSelect from 'components/form-elements/select/Select';
import DvLabel from 'components/form-elements/label/Label';
import DvButton from 'components/form-elements/button/Button';
import DvDatePicker from 'components/form-elements/date-picker/DatePicker';
import DvFormError from 'components/form-elements/form-error/FormError';

export default {
  inject: ['$validator'],
  components: {
    DvFormRow,
    DvSelect,
    DvLabel,
    DvDatePicker,
    DvButton,
    DvFormError,
  },
  computed: {
    ...mapGetters({
      inputDisabled: 'isLoading',
      isLoading: 'isLoading',
    }),
    ...mapGetters('services/lifeCycle', {
      currentLifeCycleStage: 'currentLifeCycleStage'
    }),
    lifeCycleStageTypes() {
      return [
        {
          type: 'option',
          value: this.currentLifeCycleStage.lifeCycleStageType,
          label: this.currentLifeCycleStage.lifeCycleStageTypeName,
          selected: true,
        }
      ]
    },
    from() {
      return this.localState.from !== null ?
        this.localState.from :
        this.currentLifeCycleStage.from;
    },
    to() {
      return this.localState.to !== null ?
        this.localState.to :
        this.currentLifeCycleStage.to;
    },
    inputDisabled() {
      return this.isLoading;B
    },
    buttonDisabled() {
      return this.isLoading || this.$validator.errors.any();
    },
  },
  methods: {
    updateChangedFrom(event){
      this.localState.from = event.target.value;
    },
    updateChangedTo(event){
      this.localState.to = event.target.value;
    },
    save() {
      this.$validator.validateAll()
        .then(isSucces => {
          if(isSucces){
            this.$store.dispatch(
              "services/lifeCycle/changePeriodForLifeCycleStage", {
                id: this.$router.currentRoute.params.id,
                lifeCycleStageId: this.$router.currentRoute.params.lifeCycleStageId,
                data: {
                  from: this.from,
                  to: this.to,
                }
              });
          }
        })
        .catch(error => {
          console.error(error);
        });
    }
  },
  mounted() {
    this.$store.dispatch(
      'services/lifeCycle/loadLifeCycleStage',
      {
        publicServiceId: this.$route.params.id,
        lifeCycleStageId: this.$route.params.lifeCycleStageId
      });
  },
  data() {
    return {
      localState: {
        from: null,
        to: null,
      }
    };
  },
};
</script>
