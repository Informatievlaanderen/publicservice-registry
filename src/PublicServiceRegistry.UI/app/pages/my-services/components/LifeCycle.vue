<template>
  <dv-grid class="col--10-12 col--1-1--s">
    <dv-column>
      <div class="cta-title">
        <h1 class="h2 cta-title__title">Levensloop</h1>
        <dv-route-button class="button cta-title__cta" text="Voeg levensfase toe" :to="{ name: 'my-service-new-life-cycle' }"/>
      </div>

      <dv-data-table
        ref="lifeCycleTable"
        :fields="lifeCycleFields"
        :per-page="10"
        :sort-column="sortColumn"
        :pagination="paging"
        :dataManager="dataManager"
        :isLoading="isLoading"
        track-by="id">
          <template slot="lifeCycleStage" slot-scope="props">
            <div class="custom-actions">
              {{props.rowData.lifeCycleStageTypeName}}
            </div>
          </template>

          <template slot="van" slot-scope="props">
            {{props.rowData.from | moment('DD.MM.YYYY') }}
          </template>

          <template slot="tot" slot-scope="props">
            {{props.rowData.to | moment('DD.MM.YYYY') }}
          </template>

          <template slot="actions" slot-scope="props">
            <div class="custom-actions u-align-right">
              <router-link class="vi vi-u-badge vi-u-badge--grey vi-u-badge--small vi-edit" :to="{ name: 'my-service-edit-life-cycle', params: { localId: props.rowData.localId }}"></router-link>
              <a href="" class="vi vi-u-badge vi-u-badge--grey vi-u-badge--small vi-trash"></a>
            </div>
          </template>
      </dv-data-table>
    </dv-column>
  </dv-grid>
</template>

<script>
import { mapGetters } from 'vuex';

import DvDataTable from 'components/data-table/DataTable';
import DvRouteButton from 'components/buttons/RouteButton';
import DvFormRow from 'components/form-elements/form-row/FormRow';
import DvFormGroup from 'components/form-elements/form-group/FormGroup';
import DvLabel from 'components/form-elements/label/Label';
import DvInputField from 'components/form-elements/input-field/InputField';
import DvButton from 'components/form-elements/button/Button';

export default {
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
    ...mapGetters({
      isLoading: 'isLoading',
    }),
    ...mapGetters('services', {
      lifeCycle: 'lifeCycle',
      count: 'numberOfLifeCycleStages',
      sortColumn: 'sortColumn',
      paging: 'paging',
    }),
    count() {
      return this.lifeCycle.length;
    },
  },
  mounted() {
    vl.popover.dressAll();
  },
  methods: {
    dataManager(sortOrder, paging) {
      this.$store.dispatch('services/loadLifeCycle', { sortOrder, paging, routerParams: this.$router.currentRoute.params });
    },
  },
  watch: {
    lifeCycle(lifeCycle) {
      this.$refs.lifeCycleTable.setData(lifeCycle);
    },
  },
  data() {
    return {
      lifeCycleFields: [
        {
          name: '__slot:lifeCycleStage',
          sortField: 'LifeCycleStage',
          title: 'Levensfase',
          widthPercentage: 50,
        },
        {
          name: '__slot:van',
          sortField: 'From',
          title: 'Van',
          widthPercentage: 20,
        },
        {
          name: '__slot:tot',
          sortField: 'To',
          title: 'Tot',
          widthPercentage: 20,
        },
        {
          name: '__slot:actions',
          widthPercentage: 10,
        },
      ]
    };
  },
};
</script>
