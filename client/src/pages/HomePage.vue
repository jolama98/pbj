<script setup>
import { poemsService } from '@/services/PoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { onMounted } from 'vue';

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
  <div class="home flex-grow-1 d-flex flex-column align-items-center justify-content-center">
    PBJ
  </div>
</template>

<style scoped lang="scss"></style>
