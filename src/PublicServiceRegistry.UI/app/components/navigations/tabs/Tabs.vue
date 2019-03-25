<template>
  <div class="tabs__wrapper">
    <ul :class="classes">
      <slot></slot>
    </ul>
  </div>
</template>

<script>

import DvTab from './Tab';

export default {
  components: {
    DvTab,
  },
  name: 'tabs',
  data() {
    return {
      classes: {
        tabs: true,
      },
    };
  },
};
</script>
