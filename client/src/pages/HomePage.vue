<script setup>
import { AppState } from '@/AppState.js';
import PoemCard from '@/components/PoemCard.vue';
import { poemsService } from '@/services/PoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { computed, onMounted } from 'vue';

const poems = computed(() => AppState.poems)

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
      <div class="col-12">
        <div class="masonry-layout">
          <div class="masonry-item" v-for="poem in poems" :key="poem.id">
            <PoemCard :poem />
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped lang="scss">
.masonry-layout {
  column-count: 3;
  column-gap: 1rem;
  width: 100%;
}

.masonry-item {
  break-inside: avoid;
  margin-bottom: 1rem;
}

@media (max-width: 800px) {
  .masonry-layout {
    column-count: 2;
  }
}

@media (max-width: 480px) {
  .masonry-layout {
    column-count: 1;
  }
}

</style>
