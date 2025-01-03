<script setup>
import { AppState } from '@/AppState.js';
import { Poem } from '@/models/Poem.js';
import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';


const poem = computed(() => AppState.poemById)
const myBook = computed(() => AppState.accountBooks)

const savedPoemToBookData = ref({
  poemId: 0,
  bookId: 0
})

async function createSavePoem() {
  try {
    // const poemId = props.activePoem.id
    savedPoemToBookData.value.poemId = AppState.poemById.id;
    await savePoemsService.createSavePoem(savedPoemToBookData.value)
    logger.log('Yum!', AppState.poemById.id)

    Modal.getOrCreateInstance('#poem-modal').hide()
    savedPoemToBookData.value = {
      poemId: 0,
      bookId: 0
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
        <select class="btn btn-light border border-2 dropdown-toggle rounded-4" data-bs-toggle="dropdown"
          aria-expanded="false" v-model="savedPoemToBookData.bookId">
          <option value="0" class="text-primary" disabled selected>{{ myBook.length }} books</option>
          <hr />
          <option v-for="book in myBook" :key="book.id" :book="book.id">
            {{ book.title }}
          </option>
        </select>
      </div>
      <button @click="createSavePoem()" class="mdi mdi-notebook btn btn-secondary border border-2 rounded-4">{{
        poem?.saves }}</button>
    </div>
  </div>
</template>


<style lang="scss" scoped></style>
