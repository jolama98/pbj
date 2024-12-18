<script setup>
import { Account } from '@/models/Account.js';

const props = defineProps({
  accountProps: { type: Account, required: true }
})



</script>


<template>
  <div class="container">
    <div class="card">
      <div class="card-title align-self-center pt-5">
        <h1>{{ props.accountProps?.name }}</h1>
      </div>
      <div class="card-body d-flex flex-column">
        <div class="align-self-center">
          <img class="avatar img-thumbnail mb-3" :src="props.accountProps?.picture" :alt="props.accountProps?.name">
        </div>
        <div class="flex-column d-flex">
          <button class="btn btn-outline-info mb-4  rounded-4">Edit Profile</button>
          <button class="btn btn-outline-info mb-4 rounded-4">All</button>
          <button class="btn btn-outline-info mb-4 rounded-4">Saved</button>
          <button class="btn btn-outline-info mb-4 rounded-4">Liked</button>
          <button class="btn btn-outline-info rounded-4">Filter<samp class="mdi mdi-chevron-down mb-2"></samp></button>
        </div>
      </div>
    </div>
  </div>
</template>


<style lang="scss" scoped>
.avatar {
  vertical-align: middle;
  width: 10vh;
  height: 10vh;
  border-radius: 50%;
}
</style>
