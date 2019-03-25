<template>
  <div class="cta-title">
    <h1 class="h2 cta-title__title">Nieuwe dienstverlening</h1>

    <div style="clear: left;">
      <form class="form col--4-6 col--1-1--s" @submit.prevent="save">
        <dv-form-row>
          <dv-label class="label" for="name">Naam</dv-label>
          <dv-input-field
            id="name"
            name="Naam"
            :value="name"
            @input.native="updateName"
            :disabled="inputDisabled"
            v-focus
            validation="required" />
          <dv-form-error id="Naam"></dv-form-error>
        </dv-form-row>

        <dv-form-row>
          <dv-button
            id="opslaan"
            label="Opslaan"
            type="submit"
            :mod-is-loading="isLoading"
            :disabled="buttonDisabled" />
        </dv-form-row>
      </form>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex';

import { UPDATE_NEWSERVICE_NAME } from 'store/mutation-types';

import DvFormRow from 'components/form-elements/form-row/FormRow';
import DvLabel from 'components/form-elements/label/Label';
import DvButton from 'components/form-elements/button/Button';
import DvInputField from 'components/form-elements/input-field/InputField';
import DvFormError from 'components/form-elements/form-error/FormError';

export default {
  inject: ['$validator'],
  components: {
    DvFormRow,
    DvLabel,
    DvInputField,
    DvButton,
    DvFormError,
  },
  computed: {
    ...mapGetters({
      inputDisabled: 'isLoading',
      isLoading: 'isLoading',
    }),
    buttonDisabled() {
      return this.isLoading ||
         !this.allValid;
    },
    allValid(){
      return Object.keys(this.fieldsBag).every(key => this.fieldsBag[key].valid);
    },
  },
  methods: {
    save() {
      if (!this.$validator.errors.any()) {
        this.$store.dispatch('services/save', { name: this.name });
      }
    },
    updateName(e) {
      this.name = e.target.value;
    }
  },
  data() {
    return {
      name: '',
    };
  },
};
</script>
