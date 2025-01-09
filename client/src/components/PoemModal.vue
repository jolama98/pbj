<script setup>
import { AppState } from '@/AppState.js';
import { Poem } from '@/models/Poem.js';
import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';


const accountBook = computed(() => AppState.profileBooks)
const account = computed(() => AppState.account)
const activePoem = computed(() => AppState.poemById)

const savedPoemToBookData = ref({
  poemId: 0,
  bookId: 0
})

// const props = defineProps({
//   activePoem: { type: Poem, required: true }
// })

async function createSavePoem() {
  try {
    // const poemId = props.activePoem.id
    savedPoemToBookData.value.poemId = AppState.poemById.id;
    logger.log('Pizza', savedPoemToBookData.value)
    await savePoemsService.createSavePoem(savedPoemToBookData.value)
    logger.log('Yum!', AppState.poemById.id)

    savedPoemToBookData.value = {
      poemId: 0,
      bookId: 0
    }
    Modal.getOrCreateInstance('#poem-modal').hide()
    Pop.success('Poem In Book!')
  }
  catch (error) {
    Pop.error(error);
  }
}


</script>


<template>
  <div class="row">
    <div class="col-12">
      <div class="card">
        <h2 class="card-header text-light p-3 d-flex justify-content-center">{{
          activePoem?.title }}
        </h2>
      </div>
    </div>
    <div class="col-12">
      <div class="d-flex align-items-end pt-5 card-body">
        {{ activePoem?.body }}
      </div>
    </div>
    <div class="modal-footer p-1">
      <i class="mdi mdi-eye"> {{ activePoem?.views }}</i>
      <i class="mdi mdi-heart"></i>

      <i class="mdi mdi-comment-edit"></i>




      <div class="dropdown">
        <form @submit.prevent="createSavePoem()" v-if="account" class="d-flex">
          <select v-model="savedPoemToBookData.bookId" class="form-select form-control no-round-end p-0"
            aria-label="Select a vault to add the picture to">
            <option selected value=0>Select a Book</option>
            <option v-for="book in accountBook" :key="book.id" :value="book.id">
              {{ book.title }}</option>
          </select>
          <button class="mdi mdi-notebook btn btn-secondary border border-2 rounded-4">{{
            activePoem?.saves }}</button>
        </form>
      </div>

      <!-- <div class="d-flex align-items-center">
        <form v-if="account" @submit.prevent="createVaultKeep()">
          <select title="select a vault" v-model="vaultKeepData.vaultId" class="btn dropdown-toggle"
            aria-label="Select a Vault" required>
            <option selected :value="0" disabled>Vault</option>
            <option v-for="vault in myVaults" :key="vault.id" :value="vault.id">{{ vault.name }}</option>
          </select>
          <button title="save to vault" type="submit" class="btn btn-success mx-2 text-light">save</button>
        </form>
      </div> -->


    </div>

  </div>
</template>


<style lang="scss" scoped></style>
