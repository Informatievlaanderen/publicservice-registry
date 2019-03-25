<template>
  <div class="cta-title">
    <h1 class="h2 cta-title__title">IPDC Integratie</h1>

    <div style="clear: left;">
      <form class="form col--4-6 col--1-1--s" @submit.prevent="save">
        <dv-form-row>
          <dv-label class="label" for="from">Zoek wijzigingen in IPDC sinds</dv-label>
          <dv-date-picker
            id="from"
            name="Wijzigingen sinds"
            :value="changedSince"
            @change.native="updateChangedSince"
            :validation="'required|date_format:DD.MM.YYYY'"
            :disabled="inputDisabled"
          />
          <dv-form-error id="Wijzigingen sinds"></dv-form-error>
        </dv-form-row>

        <dv-form-row>
          <dv-button
            id="opslaan"
            label="Ophalen"
            type="submit"
            :mod-is-loading="isLoading"
            :disabled="buttonDisabled"
          />
        </dv-form-row>
      </form>
    </div>

    <dv-grid>
      <div class="col--2-12 col--1-1--s">
        <ul id="example-1">
          <li v-for="item in changedProducts">
            <a @click="showProduct(item)">{{item}}</a>
          </li>
        </ul>
      </div>

      <div class="col--6-12 col--1-1--s properties__column">
        <dl>
        <dt class="properties__label">Naam: </dt><dd class="properties__data">{{ product.name }}</dd>
        <dt class="properties__label">Thema: </dt><dd class="properties__data">{{ product.theme }}</dd>
        <dt class="properties__label">Doelgroepen: </dt><dd class="properties__data">{{ product.targetAudiences }}</dd>
        <dt class="properties__label">Link naar ipdc: </dt><dd class="properties__data"><a target="_blank" :href="product.ipdcLink">{{product.ipdcLink}}</a></dd>
        <dt class="properties__label">Link naar data: </dt><dd class="properties__data"><a target="_blank" :href="product.ipdcDataLink">{{product.ipdcDataLink}}</a></dd>
        </dl>
      </div>
    </dv-grid>
  </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";

import { UPDATE_NEWSERVICE_NAME } from "store/mutation-types";

import DvFormRow from "components/form-elements/form-row/FormRow";
import DvLabel from "components/form-elements/label/Label";
import DvButton from "components/form-elements/button/Button";
import DvInputField from "components/form-elements/input-field/InputField";
import DvDatePicker from "components/form-elements/date-picker/DatePicker";
import DvFormError from "components/form-elements/form-error/FormError";

export default {
  inject: ["$validator"],
  components: {
    DvFormRow,
    DvLabel,
    DvInputField,
    DvButton,
    DvFormError,
    DvDatePicker
  },
  computed: {
    ...mapGetters({
      inputDisabled: "isLoading",
      isLoading: "isLoading"
    }),
    ...mapGetters("ipdc", {
      changedProducts: "changedProducts",
      changedSince: "changedSince",
      product: "product",
    }),
    buttonDisabled() {
      return this.isLoading || !this.allValid;
    },
    allValid() {
      return Object.keys(this.fieldsBag).every(
        key => this.fieldsBag[key].valid
      );
    }
  },
  mounted() {},
  methods: {
    updateChangedSince(e) {
      this.$store.dispatch("ipdc/updateChangedSince", e.target.value);
    },
    save() {
      if (!this.$validator.errors.any()) {
        this.$store.dispatch("ipdc/getChangedProducts", this.changedSince);
      }
    },
    showProduct(id) {
      this.$store.dispatch("ipdc/getProduct", id);
    },
  },
  data() {
    return {};
  }
};
</script>
