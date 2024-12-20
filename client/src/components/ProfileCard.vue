<script setup>
import { AppState } from '@/AppState.js';
import { Account } from '@/models/Account.js';
import { AuthService } from '@/services/AuthService.js';
import { computed } from 'vue';
import ModalWrapper from './ModalWrapper.vue';
import CreatePoemModal from './CreatePoemModal.vue';

const identity = computed(() => AppState.identity)
const account = computed(() => AppState.account)
async function login() {
  AuthService.loginWithPopup()
}
const props = defineProps({
  accountProps: { type: Account, required: true }
})

async function logout() {
  AuthService.logout()
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
                    :alt="props.accountProps?.name">
                </div>
              </div>

              <div class="card-title d-flex justify-content-center">
                <h1>{{ props.accountProps?.name }}</h1>
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

          <button class="btn btn-outline-info mb-5 rounded-4" data-bs-toggle="modal"
            data-bs-target="#create-poem">Create
            A Poem</button>
          <button class="btn btn-outline-info mb-4 rounded-4">All</button>
          <button class="btn btn-outline-info mb-4 rounded-4">Saved</button>
          <button class="btn btn-outline-info mb-4 rounded-4">Liked</button>



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
</template>


<style lang="scss" scoped>
.avatar {
  vertical-align: middle;
  width: 10vh;
  height: 10vh;
  border-radius: 50%;
}
</style>
