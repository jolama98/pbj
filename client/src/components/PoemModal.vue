<script setup>
import { AppState } from '@/AppState.js';
import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { computed, ref } from 'vue';


const poem = computed(() => AppState.poemById)

const savedPoemToBookData = ref({
  poemId: null,
  bookId: null
})

async function createSavePoem() {
  try {
    savedPoemToBookData.value.poemId = AppState.poemById.id;
    savePoemsService.createSavePoem(savedPoemToBookData.value)
    logger.log(AppState.poemById.id)

    savedPoemToBookData.value = {
      poemId: 0,
      bookId: 0
    }
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
      <i @click="createSavePoem()" role="button" class="mdi mdi-notebook">{{ poem?.saves }}</i>
      <i class="mdi mdi-comment-edit"></i>
    </div>
  </div>
</template>


<style lang="scss" scoped></style>
