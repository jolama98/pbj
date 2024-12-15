<script setup>
import { Poem } from '@/models/Poem.js';

const props = defineProps({
  poem: { type: Poem, required: true }
})

</script>


<template>
  <div class="card">
    <!-- <img class="img-fluid" :src="poem.imgUrl" :alt="poem.title" :title="poem.title"> -->
    <!-- <div class="card-img-overlay d-flex align-items-end justify-content-between"> -->
    <div class="card-body">
      <p class="card-title fs-5">{{ poem.title }}</p>
      <p class="card-text text-stop">{{ poem.body }}</p>
    </div>
  </div>
  <!-- </div> -->
</template>


<style lang="scss" scoped>
.text-stop {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 7;
  overflow: hidden;
  white-space: pre;
  text-wrap: wrap;
}
</style>