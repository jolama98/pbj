<script setup>
import { onMounted, ref } from 'vue';
import { loadState, saveState } from '../utils/Store.js';
import Login from './Login.vue';

const theme = ref(loadState('theme') || 'light')

onMounted(() => {
  document.documentElement.setAttribute('data-bs-theme', theme.value)
})

function toggleTheme() {
  theme.value = theme.value == 'light' ? 'dark' : 'light'
  document.documentElement.setAttribute('data-bs-theme', theme.value)
  saveState('theme', theme.value)
}

</script>

<template>
  <nav class="navbar navbar-expand-sm navbar-dark bg-dark px-3">
    <router-link class="navbar-brand d-flex" :to="{ name: 'Home' }">
      <div class="d-flex flex-column align-items-center">
        <img alt="logo" src="/img/cw-logo.png" height="45" />
      </div>
    </router-link>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarText"
      aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarText">

      <ul class="navbar-nav me-auto">
        <li>
          <router-link :to="{ name: 'About' }" class="btn text-success lighten-30 selectable text-uppercase">
            About
          </router-link>
        </li>
      </ul>
      <!-- LOGIN COMPONENT HERE -->
      <div>
        <button class="btn text-light" @click="toggleTheme"
          :title="`Enable ${theme == 'light' ? 'dark' : 'light'} theme.`">
          <Icon :name="theme == 'light' ? 'weather-sunny' : 'weather-night'" />
        </button>
      </div>

      <div class="box">
        <form name="search">
          <input type="text" class="input" name="txt" onmouseout="this.value = ''; this.blur();">

        </form>
        <i class="mdi mdi-magnify"></i>
      </div>

      <Login />
    </div>
  </nav>
</template>

<style scoped>
a:hover {
  text-decoration: none;
}

.nav-link {
  text-transform: uppercase;
}

.navbar-nav .router-link-exact-active {
  border-bottom: 2px solid var(--bs-success);
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
}

@media screen and (min-width: 576px) {}

.box {
  position: relative;
}

.input {
  padding: 12px;
  width: 2px;
  height: 2px;
  background: none;
  border: 4px solid;
  border-radius: 50px;
  box-sizing: border-box;
  font-family: Comic Sans MS;
  font-size: 26px;
  color: #13a028;
  outline: none;
  transition: .5s;
}

.box:hover input {
  width: 20dvh;
  background: #3b3640;
  border-radius: 10px;
}

.box i {
  position: absolute;
  top: 54%;
  left: 17px;
  transform: translate(-50%, -50%);
  font-size: 26px;
  color: #13a028;
  transition: .2s;
}

.box:hover i {
  opacity: 0;
  z-index: -1;
}
</style>
