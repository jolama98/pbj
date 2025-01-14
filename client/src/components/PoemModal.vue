<script setup>
import { AppState } from '@/AppState.js';

import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';


// const account = computed(() => AppState.account)
const accountBook = computed(() => AppState.profileBooks)
const activePoem = computed(() => AppState.poemById)

const savedPoemToBookData = ref({
  bookId: 0,
  poemId: 0
})

// const props = defineProps({
//   poem: { type: Poem, required: true }
// })



// function likePoem() {
//   try {
//     const poemId = activePoem.value.id
//     logger.log(poemId)
//     poemsService.likePoem(poemId)
//   }
//   catch (error) {
//     Pop.error("Could not like poem", 'error');
//     logger.log(error)
//   }
// }

async function createSavePoem() {
  try {
    savedPoemToBookData.value.poemId = activePoem.value.id;
    await savePoemsService.createSavePoem(savedPoemToBookData.value)
    logger.log('Yum!', activePoem.value.id)

    savedPoemToBookData.value = {
      bookId: 0,
      poemId: 0
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

      <i class="mdi mdi-heart p-1">{{ activePoem?.likes }}</i>

      <i class="mdi mdi-comment-edit"></i>




      <div class="dropdown">
        <form class="d-flex">
          <select v-model="savedPoemToBookData.bookId" class="form-select form-control no-round-end p-0"
            aria-label="Select a vault to add the picture to">
            <option selected value=0>Select a Book</option>
            <option v-for="book in accountBook" :key="book.id" :value="book.id">
              {{ book.title }}</option>
          </select>
          <button @click.prevent="createSavePoem()"
            class="btn btn-success text-light text-center no-round-start">save</button>
        </form>
      </div>
    </div>

  </div>
</template>


<style lang="scss" scoped></style>
