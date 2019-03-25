<template>
  <dv-grid>
    <dv-column>
      <div class="h2-sublink">
        <h2 class="h2">{{ serviceName }}</h2>
      </div>

      <dv-grid>
        <nav class="side-navigation col--2-12 col--1-1--s">
          <div class="side-navigation__content">
            <ul class="side-navigation__group">
              <router-link tag="li" class="side-navigation__item" active-class="side-navigation__item js-scrollspy-active" :exact="true" :to="'info'">
                <a link>Algemene informatie</a>
              </router-link>

              <router-link tag="li" class="side-navigation__item" active-class="side-navigation__item js-scrollspy-active" :exact="true" :to="'benamingen'">
                <a link>Alternatieve benamingen</a>
              </router-link>

              <router-link tag="li" class="side-navigation__item" active-class="side-navigation__item js-scrollspy-active" :exact="false" :to="{ name: 'my-service-life-cycle' }">
                <a link>Levensloop</a>
              </router-link>

              <router-link tag="li" class="side-navigation__item" style="vertical-align:bottom" active-class="side-navigation__item js-scrollspy-active" :exact="true" :to="'administratie'">
                <a link><i class="vi vi-warning" aria-hidden="true">&nbsp;</i>Administratie</a>
              </router-link>
            </ul>
          </div>
        </nav>

        <router-view class="main-navigation__content"></router-view>
      </dv-grid>
    </dv-column>
  </dv-grid>
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
  components: {
  },
  computed: {
    ...mapGetters('services', {
      myServiceName: 'currentMyServiceName',
    }),
  },
  mounted() {
    this.$store.dispatch('services/loadMyService', this.$router.currentRoute.params).then(() => {
      this.serviceName = this.myServiceName;
    });
  },
  data() {
    return {
      serviceName: '',
    };
  },
};
</script>
<style>
.side-navigation__group {
  border-top: 1px solid #e8ebee;
  padding-top: 2rem;
}

.side-navigation__content {
  border-bottom: 0px solid #fff;
}

.main-navigation__content{
    border-top: 1px solid #e8ebee;
    padding-top: 2rem;
}

</style>

