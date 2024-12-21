<script setup>
import { AppState } from '@/AppState.js';
import { computed } from 'vue';


const poem = computed(() => AppState.poemById)
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
      <i class="mdi mdi-notebook"></i>
      <i class="mdi mdi-comment-edit"></i>
    </div>
  </div>
</template>


<style lang="scss" scoped></style>
