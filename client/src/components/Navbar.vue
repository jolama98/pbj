<script setup>
import { computed, onMounted, onUnmounted, ref } from 'vue';
import { loadState, saveState } from '../utils/Store.js';
import Login from './Login.vue';

import { AuthService } from '@/services/AuthService.js';
import { AppState } from '@/AppState.js';
import Pop from '@/utils/Pop.js';
import { poemsService } from '@/services/PoemsService.js';
const identity = computed(() => AppState.identity)
const theme = ref(loadState('theme') || 'light')

async function login() {
  AuthService.loginWithPopup()
}

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
  <nav class="navbar navbar-expand-md navbar-dark bg-dark px-3">
    <router-link class="navbar-brand d-flex" :to="{ name: 'Home' }">
      <div class="d-flex flex-column align-items-center">
        <img alt="logo" src="/img/cw-logo.png" height="45" />
      </div>
    </router-link>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarText"
      aria-controls="navbarText" aria-expanded="true" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarText">

      <ul class="navbar-nav me-auto d-flex flex-row">
        <li>
          <router-link :to="{ name: 'About' }" class="btn text-success lighten-30 selectable text-uppercase">
            About
          </router-link>
        </li>
        <li class="d-md-none d-block">
          <button class="btn selectable text-success lighten-30 text-uppercase" @click="login" v-if="!identity">
            Login
          </button>
        </li>
      </ul>


      <div class="justify-content-around d-flex flex-wrap d-md-none d-block ">
        <button class="btn btn-outline-info  rounded-4" type="button" data-bs-toggle="dropdown"
          aria-expanded="false">Filter<samp class="mdi mdi-chevron-down mb-2"></samp></button>

        <ul class="dropdown-menu dropdown-menu-start ">
          <li><a class="dropdown-item" href="#">Action</a></li>
          <li><a class="dropdown-item" href="#">Another action</a></li>
          <li><a class="dropdown-item" href="#">Something else here</a></li>

        </ul>
        <button class="btn btn-outline-info  rounded-4">
          <router-link :to="{ name: 'Account' }">
            <div class="list-group-item dropdown-item list-group-item-action text-info">
              Edit Profile
            </div>
          </router-link>
        </button>

        <button class="btn btn-outline-info rounded-4">All</button>
        <button class="btn btn-outline-info rounded-4">Saved</button>
        <button class="btn btn-outline-info rounded-4">Liked</button>
      </div>





      <div>
        <button class="btn text-light" @click="toggleTheme"
          :title="`Enable ${theme == 'light' ? 'dark' : 'light'} theme.`">
          <Icon :name="theme == 'light' ? 'weather-sunny' : 'weather-night'" />
        </button>
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

.box input {
  width: 100%;
  background: #3b3640;
  border-radius: 10px;
}

.box i {
  position: absolute;
  top: 70%;
  left: 96%;
  transform: translate(-50%, -50%);
  font-size: 26px;
  color: #13a028;
  transition: .2s;
}
</style>
