<script setup>
import { ref, onMounted } from 'vue';
import { accountService } from '../services/AccountService.js';
import Pop from '../utils/Pop.js';
import { AppState } from '@/AppState.js';
import { logger } from '@/utils/Logger.js';

const editableAccountData = ref({
  name: '',
  picture: '',
  coverImg: '',
  email: ''
})

onMounted(() => {
  editableAccountData.value = { ...AppState.account }
})

async function updateAccount() {
  try {
    await accountService.updateAccount(editableAccountData.value)
    Pop.success("Changes saved to your account!")
  }
  catch (error) {
    Pop.error(error)
    logger.log(error)
  }
}
</script>


<template>
  <div class="container">
    <section class="row g-3">
      <div class="col-12">
        <h1>Update your profile</h1>
        <form @submit.prevent="updateAccount()">
          <div class="mb-3">
            <label for="name" class="form-label">Account Name</label>
            <input v-model="editableAccountData.name" type="text" class="form-control" id="name" required
              maxlength="100">
          </div>
          <div class="mb-3">
            <label for="picture" class="form-label">Account Picture</label>
            <input v-model="editableAccountData.picture" type="url" class="form-control" id="picture" required
              maxlength="500">
          </div>
          <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input v-model="editableAccountData.email" type="email" class="form-control" id="email" required
              maxlength="100">
          </div>
          <button type="submit" class="btn btn-success text-light">Submit</button>
        </form>
      </div>
    </section>
  </div>
</template>


<style lang="scss" scoped></style>
