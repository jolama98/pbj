<script setup>
import { poemsService } from '@/services/PoemsService.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { ref } from 'vue';

const poemData = ref({
  title: '',
  body: '',
  img: '',
  tags: '',
  isArchived: false,
  isLiked: false

})

async function createPoem() {
  try {
    await poemsService.createPoem(poemData.value)
    poemData.value = {
      title: '',
      body: '',
      img: '',
      tags: '',
      isArchived: false,
      isLiked: false
    }
    Modal.getOrCreateInstance('#create-poem').hide()
    Pop.success('Report submitted!')
  }
  catch (error) {
    Pop.error(error);
  }

}
</script>


<template>
  <form @submit.prevent="createPoem()">
    <div class="mb-3">
      <label for="poemTitle" class="form-label fw-bold fs-3">Poem Title</label>
      <input v-model="poemData.title" type="text" class="form-control" id="poemTitle" aria-describedby="titleHelp"
        required maxlength="20" minlength="1">
    </div>

    <div class="mb-3">
      <label for="poemBody" class="form-label fw-bold">Body</label>
      <textarea v-model="poemData.body" type="text" class="form-control" id="poemBody" required maxlength="2000"
        minlength="1"></textarea>
    </div>

    <div class="mb-3">
      <label for="tagsTitle" class="form-label">Tags</label>
      <input v-model="poemData.tags" type="text" class="form-control" id="tagsTitle" aria-describedby="tagsHelp"
        required maxlength="20" minlength="1">
    </div>

    <div class="mb-3">
      <input type="checkbox" class="form-check-input m-1" id="isPoemArchived" v-model="poemData.isArchived">
      <label class="form-check-label" for="isPoemArchived">Is poem Archived?</label>
    </div>

    <div class="col-12 mb-3">
      <label for="poem-image-url">Image Url</label>
      <input v-model="poemData.img" class="form-control" type="url" name="poem-image-url" id="poem-image-url"
        maxlength="1000" minlength="1">
    </div>
    <button type="submit" class="btn btn-outline-info">Submit</button>
  </form>


</template>


<style lang="scss" scoped></style>
