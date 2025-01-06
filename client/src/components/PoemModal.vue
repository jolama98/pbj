<script setup>
import { AppState } from '@/AppState.js';
import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';


const poem = computed(() => AppState.poemById)
const myBook = computed(() => AppState.profileBooks)
const account = computed(() => AppState.account)

const savedPoemToBookData = ref({
  poemId: null,
  bookId: null
})

async function createSavePoem() {
  try {
    savedPoemToBookData.value.poemId = poem.value.id;
    await savePoemsService.createSavePoem(savedPoemToBookData.value)
    logger.log('Yum!', AppState.poemById.id)

    Modal.getOrCreateInstance('#poem-modal').hide()
    savedPoemToBookData.value = {
      poemId: null,
      bookId: null
    }
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
          poem?.title }}
        </h2>
      </div>
    </div>
    <div class="col-12">
      <div class="d-flex align-items-end pt-5 card-body">
        {{ poem?.body }}
      </div>
    </div>
    <div class="modal-footer p-1">
      <i class="mdi mdi-eye"> {{ poem?.views }}</i>
      <i class="mdi mdi-heart"></i>

      <i class="mdi mdi-comment-edit"></i>

      <div class="dropdown">
        <form @submit.prevent="createSavePoem()" v-if="account" class="d-flex">
          <select v-model="savedPoemToBookData.bookId" class="form-select form-control no-round-end p-0"
            aria-label="Select a vault to add the picture to">
            <option selected value=null>Select a Book</option>
            <option v-for="book in myBook" :key="book.id" :value="book.id">
              {{ book.title }}</option>
          </select>
          <button class="mdi mdi-notebook btn btn-secondary border border-2 rounded-4">{{
            poem?.saves }}</button>
        </form>
      </div>


    </div>

  </div>
</template>


<style lang="scss" scoped></style>
