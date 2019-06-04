<template>
  <dv-grid>
    <dv-column>
      <div class="h2-sublink">
        <dv-route-button class="button cta-title__cta" text="Dienstverlening wijzigen" :to="{ name: 'my-service', id: myServiceId }"/>
        <h2 class="h2">{{ myServiceName }}</h2>
      </div>

      <div class="properties">
        <div class="properties__title">Algemene info</div>
        <dl>
          <dt class="properties__label">Bevoegde organisatie: </dt><dd class="properties__data">{{ myServiceCompetentAuthority }}</dd>
          <dt class="properties__label">Is subsidie: </dt><dd class="properties__data">{{ myServiceIsSubsidy }}</dd>
          <dt class="properties__label">Status: </dt><dd class="properties__data">{{ myServiceLifeCycleStageTypeName }}</dd>
          <dt class="properties__label">Ipdc Code: </dt>
          <dd v-if="myServiceIpdcCode" class="properties__data">
            <a target="_blank" :href="myServiceIpdcCodeLink">{{ myServiceIpdcCode }}</a>
          </dd>
          <dt class="properties__label">Wetgevende Basis: </dt>
          <dd v-if="myServiceLegislativeDocumentId" class="properties__data">
            <a target="_blank" :href="myServiceLegislativeDocumentIdLink">{{ myServiceLegislativeDocumentId }}</a>
          </dd>
        </dl>
      </div>

      <div class="properties">
        <div class="properties__title">Alternatieve benamingen</div>
        <dl>
          <div :key="labelType" v-for="labelType in labelTypes">
            <dt class="properties__label">{{labelType}}</dt><dd class="properties__data">{{ valueFor(labelType) }}</dd>
          </div>
        </dl>
      </div>
    </dv-column>
  </dv-grid>
</template>

<script>
import { mapGetters } from 'vuex';

import DvRouteButton from 'components/buttons/RouteButton';

export default {
  components: {
    DvRouteButton,
  },
  computed: {
    ...mapGetters('parameters', {
      labelTypes: 'labelTypes',
    }),
    ...mapGetters('services', {
      myServiceId: 'currentMyServiceId',
      myServiceName: 'currentMyServiceName',
      myServiceCompetentAuthority: 'currentMyServiceCompetentAuthority',
      myServiceIsSubsidy: 'currentMyServiceIsSubsidy',
      myServiceLifeCycleStageTypeName: 'currentMyServiceCurrentLifeCycleStageTypeName',
      myServiceIpdcCode: 'currentMyServiceIpdcCode',
      myServiceLegislativeDocumentId: 'currentMyServiceLegislativeDocumentId',
      alternativeLabels: 'alternativeLabels',
    }),
    myServiceIpdcCodeLink() {
      return `https://productencatalogus.vlaanderen.be/fiche/${this.myServiceIpdcCode}`;
    },
    myServiceLegislativeDocumentIdLink() {
      return `https://codex.vlaanderen.be/Zoeken/Document.aspx?DID=${this.myServiceLegislativeDocumentId}&param=informatie`;
    }
  },
  methods: {
    valueFor(labelType) {
      return this.alternativeLabels[labelType];
    },
  },
  mounted() {
    this.$store.dispatch('services/loadMyService', this.$router.currentRoute.params);
    this.$store.dispatch('services/loadAlternativeLabels', this.$router.currentRoute.params);
  },
};
</script>
<style>
.h1.cta-title__title {
  margin-bottom: 20px;
}

</style>
