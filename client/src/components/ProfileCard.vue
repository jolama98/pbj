<script setup>
import { AppState } from '@/AppState.js';
import { AuthService } from '@/services/AuthService.js';
import { computed } from 'vue';
import ModalWrapper from './ModalWrapper.vue';
import CreatePoemModal from './CreatePoemModal.vue';
import CreateBookModal from './CreateBookModal.vue';
import Pop from '@/utils/Pop.js';
import { poemsService } from '@/services/PoemsService.js';


const identity = computed(() => AppState.identity)
const account = computed(() => AppState.account)
const likedPoems = computed(() => AppState.likedPoem)
const books = computed(() => AppState.profileBooks)
const savedPoems = computed(() => AppState.savedPoem)


async function login() {
  AuthService.loginWithPopup()
}

async function logout() {
  AuthService.logout()
}

function loadSavedPoems() {
  console.log(savedPoems.value)
}

</script>


<template>
  <div class="container">
    <div class="card">

      <div class="card-body d-flex flex-column">
        <div class="align-self-center">

          <button class="btn selectable text-success lighten-30 text-uppercase my-2 my-lg-0" @click="login"
            v-if="!identity">
            Login
          </button>

          <div v-else>
            <div class="dropdown my-2 my-lg-0">
              <div type="button" class="bg-transparent border-0 selectable no-select" data-bs-toggle="dropdown"
                aria-expanded="false">
                <div v-if="account?.picture || identity?.picture">
                  <img class="avatar img-thumbnail mb-3" :src="account?.picture || identity?.picture"
                    :alt="account?.name">
                </div>
              </div>

              <div class="card-title d-flex justify-content-center">
                <h1>{{ account?.name }}</h1>
              </div>
              <div class="dropdown-menu dropdown-menu-sm-end dropdown-menu-start p-0" aria-labelledby="authDropdown">
                <div class="list-group">
                  <div class="list-group-item dropdown-item list-group-item-action text-danger selectable"
                    @click="logout">
                    <i class="mdi mdi-logout"></i>
                    logout
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="dropdown-menu dropdown-menu-sm-end dropdown-menu-start p-0" aria-labelledby="authDropdown">
            <div class="list-group">
              <div class="list-group-item dropdown-item list-group-item-action text-danger selectable" @click="logout">
                <i class="mdi mdi-logout"></i>
                logout
              </div>
            </div>
          </div>
        </div>
        <div class="flex-column d-flex">
          <button class="btn btn-outline-info mb-4  rounded-4">
            <router-link :to="{ name: 'Account' }">
              <div class="list-group-item dropdown-item list-group-item-action text-info">
                Edit Profile
              </div>
            </router-link>
          </button>

          <button class="btn btn-outline-info mb-2 rounded-4" data-bs-toggle="modal"
            data-bs-target="#create-poem">Create
            A Poem</button>

          <button class="btn btn-outline-info mb-5 rounded-4" data-bs-toggle="modal"
            data-bs-target="#create-book">Create
            A Book</button>

          <button class="btn btn-outline-info mb-4 rounded-4">{{ books.length }} Book</button>
          <button class="btn btn-outline-info mb-4 rounded-4">All</button>

          <button @click="loadSavedPoems()" class="btn btn-outline-info mb-4 rounded-4">{{ savedPoems.length }}
            Saved</button>

          <button class="btn btn-outline-info button-color mb-4 rounded-4">{{ likedPoems.length }} Liked</button>




          <button class="btn btn-outline-info rounded-4" type="button" data-bs-toggle="dropdown"
            aria-expanded="false">Filter<samp class="mdi mdi-chevron-down mb-2"></samp></button>

          <ul class="dropdown-menu dropdown-menu-end">
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
            <li><a class="dropdown-item" href="#">Something else here</a></li>

          </ul>
        </div>
      </div>
    </div>
  </div>
  <ModalWrapper id="create-poem">
    <CreatePoemModal />
  </ModalWrapper>
  <ModalWrapper id="create-book">
    <CreateBookModal />
  </ModalWrapper>
</template>


<style lang="scss" scoped>
.avatar {
  vertical-align: middle;
  width: 10vh;
  height: 10vh;
  border-radius: 50%;
}

.button-color {
  color: #13a028;
}
</style>
