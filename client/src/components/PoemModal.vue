<script setup>
import { AppState } from '@/AppState.js';
import { Poem } from '@/models/Poem.js';
import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';


const poem = computed(() => AppState.poemById)

const savedPoemToBookData = ref({
  poemId: 0,
  bookId: 0
})

const props = defineProps({
  activePoem: { type: Poem, required: true }
})

async function createSavePoem() {
  try {
    logger.log(poem.value.id, 'Yum!')

    // const poemId = props.activePoem.id
    savedPoemToBookData.value.poemId = poem.value.id;
    await savePoemsService.createSavePoem(savedPoemToBookData.value)

    Modal.getOrCreateInstance('#poem-modal').hide()
    savedPoemToBookData.value = {
      poemId: 0,
      bookId: 0
    }
    // Pop.success('Poem In Book!')
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
      <i class="mdi mdi-eye"> {{ props.activePoem?.views }}</i>
      <i class="mdi mdi-heart"></i>
      <i @click="createSavePoem()" role="button" class="mdi mdi-notebook">{{ props.activePoem?.saves }}</i>
      <i class="mdi mdi-comment-edit"></i>
    </div>
  </div>
</template>


<style lang="scss" scoped></style>
