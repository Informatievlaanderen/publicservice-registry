<template>
  <div class="grid">
    <div class="col--1-1">
      <h1 class="h2">Systeem</h1>
      <ul class="grid grid--is-stacked">
        <li class="col--1-3 col--1-1--s">
          <router-link tag="a" class="tile" :exact="true" :to="'systeem/ipdc'">
            <div class="tile__title">Ipdc integratie</div>
          </router-link>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
export default {};
</script>
<style scoped>
.tile {
  display: block;
  color: #333332;
  text-decoration: none;
  background-color: #e8ebee;
}

.tile__title {
  font-weight: 500;
  font-size: 1.6rem;
  color: #06c;
  margin-bottom: 1rem;
}

.doormat,
.paragraph-link,
.tile {
  position: relative;
  border-left: 1.6rem #06c solid;
  padding: 1.5rem;
}

.doormat:before,
.paragraph-link:before,
.tile:before {
  left: -1.4rem;
  top: 1.5rem;
}

.doormat:focus,
.doormat:hover,
.paragraph-link:focus,
.paragraph-link:hover,
.tile:focus,
.tile:hover {
  border-left-color: #3184d6;
}
</style>
