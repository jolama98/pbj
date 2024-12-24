<script setup>
import { AppState } from '@/AppState.js';
import PoemCard from '@/components/PoemCard.vue';
import ProfileCard from '../components/ProfileCard.vue';
import { poemsService } from '@/services/PoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { computed, onMounted, onUnmounted, ref } from 'vue';
import ModalWrapper from '@/components/ModalWrapper.vue';
import PoemModal from '@/components/PoemModal.vue';

const poems = computed(() => AppState.poems)
const account = computed(() => AppState.account)



onMounted(() => {
  getAllPoems()
})



async function getAllPoems() {
  try {
    await poemsService.getAllPoems()
  }
  catch (error) {
    Pop.error(error)
    logger.log
  }
}
</script>

<template>
  <div class="container-fluid">
    <section class="row">


      <div class="d-flex flex-row justify-content-center">
        <div class="col-md-3 pt-3">
          <div class="d-none d-md-block">
            <ProfileCard :accountProps="account" />
          </div>
        </div>
        <div class="col-md-8 col-12 pt-3">
          <div v-for="poem in poems" :key="poem.id" class="pb-3">
            <PoemCard :poem="poem" data-bs-toggle="modal" data-bs-target="#poem-modal" />
          </div>

        </div>
      </div>
    </section>
  </div>

  <ModalWrapper id="poem-modal">
    <PoemModal />
  </ModalWrapper>
</template>

<style scoped lang="scss"></style>

// .masonry-layout {
// column-count: 3;
// column-gap: 1rem;
// width: 100%;
// }

// .masonry-item {
// break-inside: avoid;
// margin-bottom: 1rem;
// }

// @media (max-width: 800px) {
// .masonry-layout {
// column-count: 2;
// }
// }

// @media (max-width: 480px) {
// .masonry-layout {
// column-count: 1;
// }
// }
