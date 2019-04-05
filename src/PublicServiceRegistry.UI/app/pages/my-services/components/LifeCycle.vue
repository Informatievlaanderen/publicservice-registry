<template>
  <dv-grid class="col--10-12 col--1-1--s">
    <div
      class="overlay"
      v-if="removal.showConfirmation">
      <div
        class="modal-dialog"
        tabindex="-1"
        role="document"
        aria-hidden="true">
        <h2 class="modal-dialog__title">
          Levensloopfase verwijderen?
        </h2>
        <div class="modal-dialog__content">
          Bent u zeker dat u deze levensloopfase wilt verwijderen? Deze actie kan niet ongedaan worden.
        </div>
        <div class="modal-dialog__buttons">
          <a href="#" @click.prevent="confirmRemoval" role="button" class="button modal-dialog__button">Verwijderen</a>
          <a href="#" @click.prevent="cancelRemoval" role="button" class="modal-dialog__button"><i class="vi vi-cross vi-u-link"></i>Annuleren</a>
        </div>
      </div>
    </div>

    <dv-column>
      <div class="cta-title">
        <h2 class="h3 cta-title__title">Levensloop</h2>
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
          <template slot="lifeCycleStageType" slot-scope="props">
            <div class="custom-actions">
              {{ props.rowData.lifeCycleStageTypeName }}
            </div>
          </template>

          <template slot="van" slot-scope="props">
            {{ props.rowData.from }}
          </template>

          <template slot="tot" slot-scope="props">
            {{ props.rowData.to }}
          </template>

          <template slot="actions" slot-scope="props">
            <div class="custom-actions u-align-right">
              <router-link class="vi vi-u-badge vi-u-badge--grey vi-u-badge--small vi-edit" :to="{ name: 'my-service-edit-life-cycle', params: { lifeCycleStageId: props.rowData.lifeCycleStageId }}"></router-link>
              <a href="" @click.prevent="askConfirmationForRemoval(props.rowData.lifeCycleStageId)" class="vi vi-u-badge vi-u-badge--grey vi-u-badge--small vi-trash"></a>
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
    ...mapGetters('services/lifeCycle', {
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
      this.$store.dispatch('services/lifeCycle/loadLifeCycle', { sortOrder, paging, id: this.$router.currentRoute.params.id });
    },
    askConfirmationForRemoval(lifeCycleStageId) {
      this.removal = {
        showConfirmation: true,
        lifeCycleStageId: lifeCycleStageId
      };
    },
    cancelRemoval() {
      this.removal.showConfirmation = false;
    },
    confirmRemoval() {
      this.$store.dispatch(
        "services/lifeCycle/removeLifeCycleStage", {
          id: this.$router.currentRoute.params.id,
          lifeCycleStageId: this.removal.lifeCycleStageId,
        }).finally(() => {
          this.removal.showConfirmation = false;
          this.$store.dispatch('services/lifeCycle/loadLifeCycle',
            { id: this.$router.currentRoute.params.id });
        });
    }
  },
  watch: {
    lifeCycle(lifeCycle) {
      this.$refs.lifeCycleTable.setData(lifeCycle);
    },
  },
  data() {
    return {
      removal: {
        showConfirmation: false,
      },
      lifeCycleFields: [
        {
          name: '__slot:lifeCycleStageType',
          sortField: 'LifeCycleStageType',
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
