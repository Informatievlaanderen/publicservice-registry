<template>
  <dv-grid>
    <dv-column>
      <div class="cta-title">
        <h1 class="h2 cta-title__title">Alle dienstverleningen</h1>
        <dv-route-button class="button cta-title__cta" text="Voeg dienstverlening toe" :to="{ name: 'new-service' }"/>
      </div>

      <dv-services-filter
        v-on:filter="filter"
        :is-loading="isLoading" />

      <div class="data-table__actions data-table__actions--top">
        <div class="grid">
          <div class="col--6-12 col--9-12--xs">
            <ul class="data-table__actions__list">
              <li class="data-table__action">
                <div class="popover popover--left js-popover">
                  <button class="data-table__action__toggle data-table__action__toggle--arrow js-popover__toggle">Downloaden</button>
                  <ul class="popover__content">
                    <li>
                      <a class="popover__link" @click="exportCsv" href="#">CSV file</a>
                    </li>
                  </ul>
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>

      <dv-data-table
        ref="allServicesTable"
        :fields="myServiceFields"
        :per-page="10"
        :sort-column="sortColumn"
        :pagination="paging"
        :dataManager="dataManager"
        :isLoading="isLoading"
        track-by="id">
          <template slot="id-field" slot-scope="props">
            <div class="custom-actions">
              <router-link :to="{ name: 'my-service-view', params: { id: props.rowData.id }}">
                {{props.rowData.id}}
              </router-link>
            </div>
          </template>
          <template slot="naam" slot-scope="props">
            {{props.rowData.naam}}
          </template>
          <template slot="competent-authority-field" slot-scope="props">
            <div class="custom-actions">
              <a
                target="_blank"
                :href="toDataVlaanderen(props.rowData.verantwoordelijkeAutoriteitCode)" >
                {{props.rowData.verantwoordelijkeAutoriteitNaam}}
              </a>
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

import DvServicesFilter from './Filter';

export default {
  components: {
    DvRouteButton,
    DvDataTable,
    DvFormRow,
    DvFormGroup,
    DvLabel,
    DvInputField,
    DvButton,
    DvServicesFilter
  },
  computed: {
    ...mapGetters({
      isLoading: 'isLoading',
    }),
    ...mapGetters('services', {
      allServices: 'allServices',
      count: 'numberOfServices',
      sortColumn: 'sortColumn',
      paging: 'paging',
    }),
    count() {
      return this.allServices.length;
    },
  },
  mounted() {
    vl.popover.dressAll();
  },
  methods: {
    // eslint-disable-next-line
    dataManager(sortOrder, paging) {
      this.$store.dispatch('services/getAllServices', { sortOrder, paging, routerParams: this.$router.currentRoute.params });
    },
    filter(filterData) {
      this.$store.dispatch('services/filterServices', filterData)
    },
    toDataVlaanderen(ovoCode) {
      return `https://data.vlaanderen.be/id/organisatie/${ovoCode}`;
    },
    exportCsv()  {
      this.$store.dispatch('services/exportServicesAsCsv');
    },
  },
  watch: {
    // Why the watch? Watch this https://github.com/ratiw/vuetable-2/issues/109#issuecomment-301467095.
    allServices(allServices) {
      this.$refs.allServicesTable.setData(allServices);
    },
  },
  data() {
    return {
      myServiceFields: [
        {
          name: '__slot:id-field',
          sortField: 'PublicserviceId',
          title: 'DVR Code',
          widthPercentage: 15,
        },
        {
          name: '__slot:naam',
          sortField: 'Name',
          title: 'Naam',
          widthPercentage: 35,
        },
        {
          name: '__slot:competent-authority-field',
          sortField: 'CompetentAuthorityName',
          title: 'Verantwoordelijke organisatie',
          widthPercentage: 35,
        },
      ]
    };
  },
};
</script>
