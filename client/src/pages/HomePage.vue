<script setup>
import { AppState } from '@/AppState.js';
import PoemCard from '@/components/PoemCard.vue';
import ProfileCard from '../components/ProfileCard.vue';
import { poemsService } from '@/services/PoemsService.js';

import Pop from '@/utils/Pop.js';
import { computed, onMounted, ref, watch } from 'vue';
import ModalWrapper from '@/components/ModalWrapper.vue';
import PoemModal from '@/components/PoemModal.vue';


const poems = computed(() => AppState.poems)


watch(() => AppState.poemById, () => {
  getAllPoems()
})


// // watch works directly on a ref
// watch(question, async (newQuestion, oldQuestion) => {
//   if (newQuestion.includes('?')) {
//     loading.value = true
//     answer.

onMounted(() => {
  getAllPoems()
})


async function getAllPoems() {
  try {
    await poemsService.getAllPoems()
  }
  catch (error) {
    Pop.error(error)
  }
}


</script>

<template>
  <div class="container-fluid">
    <section class="row">
      <div class="d-flex flex-row justify-content-center">

        <div class="col-md-3 pt-3">
          <div class="d-none d-md-block">
            <ProfileCard />
          </div>
        </div>

        <div class="col-md-8 col-12 pt-3 scrollable-div">
          <div v-for="poem in poems" :key="poem.id" class="pb-3 ">
            <PoemCard :poem="poem" />
          </div>
        </div>

      </div>
    </section>
  </div>

  <ModalWrapper id="poem-modal">
    <PoemModal />
  </ModalWrapper>

</template>

<style scoped lang="scss">
.scrollable-div {
  width: 600px;
  /* Set a fixed width */
  height: 900px;
  /* Set a fixed height */
  overflow: auto;
  /* Enables scrollbars when content overflows */

}
</style>
