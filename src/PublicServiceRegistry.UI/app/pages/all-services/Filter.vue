<template>
  <div class="">
    <form @submit.prevent="filter">
      <dv-grid>
        <div class="col--6-12 col--1-1--s">
          <dv-form-group>
            <dv-form-row>
              <dv-label class="label" for="dvrCode">DVR Code</dv-label>
              <dv-input-field
                id="dvrCode"
                name="DvrCode"
                :value="dvrCode"
                @input.native="updateDvrCode"
                v-focus />
            </dv-form-row>

            <dv-form-row>
              <dv-label class="label" for="serviceName">Naam</dv-label>
              <dv-input-field
                id="serviceName"
                name="DienstNaam"
                :value="serviceName"
                @input.native="updateServiceName" />
            </dv-form-row>
          </dv-form-group>
        </div>

        <div class="col--6-12 col--1-1--s">
          <dv-form-group>
            <dv-form-row>
              <dv-label class="label" for="competentAuthority">Verantwoordelijke organisatie</dv-label>
              <dv-input-field
                id="competentAuthority"
                name="VerantwoordelijkeAuthoriteit"
                :value="competentAuthority"
                @input.native="updateCompetentAuthority" />
            </dv-form-row>
          </dv-form-group>
        </div>

        <div class="col--1-1">
          <dv-form-group>
            <dv-form-row>
              <div class="form__buttons">
                <div class="form__buttons__right">
                  <dv-button
                    v-show="filterChanged"
                    label="Filter wissen"
                    :mod-is-loading="isLoading"
                    type="button"
                    @click.native="clearFilter" />
                  <dv-button
                    label="Zoeken"
                    :mod-is-loading="isLoading"
                    type="submit" />
                </div>
              </div>
            </dv-form-row>
          </dv-form-group>
        </div>
      </dv-grid>
    </form>
  </div>
</template>

<script>
import DvDataTable from 'components/data-table/DataTable';
import DvRouteButton from 'components/buttons/RouteButton';
import DvFormRow from 'components/form-elements/form-row/FormRow';
import DvFormGroup from 'components/form-elements/form-group/FormGroup';
import DvLabel from 'components/form-elements/label/Label';
import DvInputField from 'components/form-elements/input-field/InputField';
import DvButton from 'components/form-elements/button/Button';

import { mapGetters } from 'vuex';

export default {
  name: 'services-filter',
  components: {
    DvRouteButton,
    DvDataTable,
    DvFormRow,
    DvFormGroup,
    DvLabel,
    DvInputField,
    DvButton
  },
  computed: {
    filterChanged: function() {
      return this.dvrCode !== '' ||
        this.serviceName !== '' ||
        this.competentAuthority !== '';
    }
  },
  methods: {
    updateDvrCode(e) {
      this.dvrCode = e.target.value;
    },
    updateServiceName(e) {
      this.serviceName = e.target.value;
    },
    updateCompetentAuthority(e) {
      this.competentAuthority = e.target.value;
    },
    filter() {
      this.$emit('filter',
      {
        dvrCode: this.dvrCode,
        serviceName: this.serviceName,
        competentAuthority: this.competentAuthority,
      })
    },
    clearFilter() {
      this.dvrCode = this.serviceName = this.competentAuthority = '';
      this.filter();
    }
  },
  props: {
    isLoading: {
      default: false,
      type: Boolean,
    },
  },
  data() {
    return {
      dvrCode: '',
      serviceName: '',
      competentAuthority: '',
    };
  },
};
</script>

<style scoped>
</style>
