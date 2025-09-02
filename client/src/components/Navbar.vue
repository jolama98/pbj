<script setup>
import { computed, onMounted, onUnmounted, ref } from 'vue'
import { loadState, saveState } from '../utils/Store.js'
import Login from './Login.vue'

import { AuthService } from '@/services/AuthService.js'
import { AppState } from '@/AppState.js'
import Pop from '@/utils/Pop.js'
import { poemsService } from '@/services/PoemsService.js'

const identity = computed(() => AppState.identity)
const theme = ref(loadState('theme') || 'light')
const searchQuery = ref('')

let debounceId = null

function login() {
  AuthService.loginWithPopup()
}

onMounted(() => {
  document.documentElement.setAttribute('data-bs-theme', theme.value)
})

onUnmounted(() => {
  poemsService.clearSearchQuery()
  if (debounceId) clearTimeout(debounceId)
})

function searchPoem() {
  try {
    poemsService.searchPoem(searchQuery.value)
  } catch (error) {
    Pop.error(error)
  }
}

// small debounce for type-to-search UX
function onSearchInput() {
  if (debounceId) clearTimeout(debounceId)
  debounceId = setTimeout(() => {
    searchPoem()
  }, 300)
}

function clearSearch() {
  searchQuery.value = ''
  poemsService.clearSearchQuery()
}

function toggleTheme() {
  theme.value = theme.value === 'light' ? 'dark' : 'light'
  document.documentElement.setAttribute('data-bs-theme', theme.value)
  saveState('theme', theme.value)
}
</script>

<template>
  <nav class="navbar navbar-expand-md shadow-sm glass-nav sticky-top" aria-label="Main navigation">
    <div class="container-xxl">
      <!-- Brand -->
      <router-link class="navbar-brand d-flex align-items-center gap-2" :to="{ name: 'Home' }">
        <span class="brand-dot" aria-hidden="true"></span>
        <span class="fw-semibold text-uppercase">PBJ</span>
      </router-link>

      <!-- Toggler -->
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarMain"
        aria-controls="navbarMain" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <!-- Collapsible content -->
      <div class="collapse navbar-collapse" id="navbarMain">
        <!-- Left links -->
        <ul class="navbar-nav me-3">
          <li class="nav-item">
            <router-link :to="{ name: 'Home' }" class="nav-link text-uppercase">Home</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ name: 'About' }" class="nav-link text-uppercase">About</router-link>
          </li>
        </ul>

        <!-- Search -->
        <form class="search ms-md-0 me-auto my-2 my-md-0" @submit.prevent="searchPoem" role="search"
          aria-label="Search poems">
          <div class="input-group search__group">
            <span class="input-group-text search__icon" id="search-addon" aria-hidden="true">
              <i class="mdi mdi-magnify"></i>
            </span>
            <input v-model="searchQuery" @input="onSearchInput" type="search" class="form-control search__input"
              placeholder="Search poems…" aria-label="Search poems" aria-describedby="search-addon" name="query"
              id="query" autocomplete="off" />
            <button v-if="searchQuery" class="btn btn-outline-secondary search__clear" type="button"
              @click="clearSearch" aria-label="Clear search" title="Clear">
              <i class="mdi mdi-close"></i>
            </button>
          </div>
        </form>

        <!-- Right controls -->
        <ul class="navbar-nav align-items-md-center gap-2">
          <!-- Filters (mobile & desktop) -->
          <li class="nav-item dropdown">
            <button class="btn btn-outline-info rounded-4 dropdown-toggle" data-bs-toggle="dropdown"
              aria-expanded="false" aria-label="Open filters">
              Filters
            </button>
            <ul class="dropdown-menu dropdown-menu-end">
              <li><button class="dropdown-item" type="button">All</button></li>
              <li><button class="dropdown-item" type="button">Saved</button></li>
              <li><button class="dropdown-item" type="button">Liked</button></li>
              <li>
                <hr class="dropdown-divider" />
              </li>
              <li><button class="dropdown-item" type="button">Edit Profile</button></li>
            </ul>
          </li>

          <!-- Account link (as button for consistent sizing) -->
          <li class="nav-item d-none d-md-block">
            <router-link :to="{ name: 'Account' }" class="btn btn-outline-info rounded-4">
              Edit Profile
            </router-link>
          </li>

          <!-- Login (mobile only if not authenticated) -->
          <li class="nav-item d-md-none" v-if="!identity">
            <button class="btn btn-success rounded-4" @click="login">Login</button>
          </li>

          <!-- Theme toggle -->
          <li class="nav-item">
            <button class="btn btn-light-subtle theme-toggle" @click="toggleTheme"
              :title="`Enable ${theme === 'light' ? 'dark' : 'light'} theme`" aria-label="Toggle color theme">
              <i :class="['mdi', theme === 'light' ? 'mdi-weather-night' : 'mdi-weather-sunny']"></i>
            </button>
          </li>

          <!-- Auth widget -->
          <li class="nav-item">
            <Login />
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<style scoped lang="scss">
/* --- Shell & brand --- */
.glass-nav {
  backdrop-filter: saturate(120%) blur(6px);
  background: color-mix(in oklab, var(--bs-body-bg) 82%, transparent);
}

.navbar {
  --_pad-y: .6rem;
  padding-top: var(--_pad-y);
  padding-bottom: var(--_pad-y);
}

.brand-dot {
  width: .9rem;
  height: .9rem;
  border-radius: 50%;
  display: inline-block;
  background: linear-gradient(135deg, var(--bs-primary), var(--bs-info));
  box-shadow: 0 0 0 3px color-mix(in oklab, var(--bs-primary) 20%, transparent);
}

/* --- Links --- */
.nav-link {
  letter-spacing: .5px;
}

.navbar-nav .router-link-exact-active {
  position: relative;
}

.navbar-nav .router-link-exact-active::after {
  content: '';
  position: absolute;
  left: .5rem;
  right: .5rem;
  bottom: .25rem;
  height: 2px;
  background: var(--bs-success);
  border-radius: 2px;
}

/* --- Search --- */
.search {
  min-width: min(520px, 100%);
}

.search__group {
  border-radius: 999px;
  overflow: hidden;
  box-shadow: inset 0 0 0 1px rgba(0, 0, 0, .05);
}

.search__icon {
  background: transparent;
  border: 0;
  font-size: 1.1rem;
  padding-left: .9rem;
  padding-right: .25rem;
}

.search__input {
  border: 0 !important;
  background: color-mix(in oklab, var(--bs-body-bg) 85%, transparent);
  height: 42px;
  font-size: .95rem;
  line-height: 1;
  padding-top: .6rem;
  padding-bottom: .6rem;
}

.search__input:focus {
  box-shadow: none;
  outline: none;
}

.search__group:focus-within {
  box-shadow:
    inset 0 0 0 2px color-mix(in oklab, var(--bs-info) 55%, transparent),
    0 6px 16px rgba(0, 0, 0, .06);
}

.search__clear {
  border: 0;
  height: 42px;
}

/* --- Theme toggle --- */
.theme-toggle {
  border-radius: 999px;
  border: 1px solid color-mix(in oklab, var(--bs-body-color) 10%, transparent);
  width: 42px;
  height: 42px;
  display: inline-grid;
  place-items: center;
  transition: transform .15s ease;
}

.theme-toggle:hover {
  transform: translateY(-1px);
}

/* --- Dropdown polish --- */
.dropdown-menu {
  border-radius: .75rem;
  box-shadow: 0 10px 24px rgba(0, 0, 0, .08);
}

/* --- Small screens --- */
@media (max-width: 767.98px) {
  .search {
    min-width: 100%;
    margin: .5rem 0 1rem;
  }
}
</style>
