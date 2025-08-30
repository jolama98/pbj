<script setup>
import { AppState } from '@/AppState.js';
import { Poem } from '@/models/Poem.js';
import { likedPoemService } from '@/services/LikedPoemService.js';
import { poemsService } from '@/services/PoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed } from 'vue';

const account = computed(() => AppState.account)


const props = defineProps({
  poem: { type: Poem, required: true }
})

async function likePoem() {
  try {
    const poemId = { poemId: props.poem.id }
    await likedPoemService.createLikePoem(poemId)
  }
  catch (error) {
    Pop.error("Could not like poem", 'error');
    logger.log(error)
  }
}

async function setActivePoem(poemId) {
  try {
    await poemsService.getPoemById(poemId)
  }
  catch (error) {
    Pop.error(error);
  }
}

async function deletePoem() {
  try {
    const choice = await Pop.confirm("Are You Sure???", 'Delete Poem')
    if (choice == false) {
      Pop.toast("action canceled successfully", 'info', 'center')
      return
    }
    Modal.getOrCreateInstance('#poem-modal').hide()
    await poemsService.deletePoem(props.poem.id)
    Pop.success("Poem Deleted!")
  }
  catch (error) {
    Pop.error(error);
  } 
}

</script>
<template>
  <div class="card">

      <div v-if="poem" class="container-fluid">
          <div class="card-body" data-bs-toggle="modal" data-bs-target="#poem-modal" role="button"
          @click="setActivePoem(props.poem.id)">
          <div class="d-flex align-items-center justify-content-between">
            <h5 class="card-title ">{{ props.poem.title }}</h5>
            <i v-if="props.poem.authorId == account?.id" type="button" @click="deletePoem()"
            class="mdi mdi-close-octagon-outline fs-1 text-danger">
          </i>
        </div>
        <p class="card-text text-stop">{{ props.poem.body }}</p>
      </div>
      
      
      <div class="card-footer justify-content-end d-flex">
        <small class="text-body-secondary">
          <i class="mdi mdi-eye p-1"> {{ props.poem.views }}</i></small>
          <small class="text-body-secondary">
            <i class="mdi mdi-sack p-1"> {{ props.poem.saves }}</i></small>
            <small class="text-body-secondary">
              <i @click="likePoem()" class="mdi mdi-heart p-1">{{  }}</i></small>
              <!-- // FIXME - this will be the array of LikedPoem objects -->

            </div>
          </div>

  </div>
</template>


<style lang="scss" scoped>
.text-stop {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 4;
  overflow: hidden;
  white-space: pre;
  text-wrap: wrap;
}
</style>
