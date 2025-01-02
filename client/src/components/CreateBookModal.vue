<script setup>
import { bookService } from '@/services/BookService.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { ref } from 'vue';


const bookData = ref({
  title: '',
  isPrivate: false,
})

async function createBook() {
  try {
    await bookService.createBook(bookData.value)
    bookData.value = {
      title: '',
      isPrivate: false,
    }
    Modal.getOrCreateInstance('#create-book').hide()
    Pop.success('Report submitted!')
  }
  catch (error) {
    Pop.error(error);
  }
}
</script>


<template>
  <form @submit.prevent="createBook()">
    <div class="mb-3">
      <label for="bookTitle" class="form-label fw-bold fs-3">Book Title</label>
      <input v-model="bookData.title" type="text" class="form-control" id="bookTitle" aria-describedby="titleHelp"
        required maxlength="20">
    </div>
    <div class="mb-3">
      <input type="checkbox" class="form-check-input m-1" id="isBookPrivate" v-model="bookData.isPrivate">
      <label class="form-check-label" for="isBookPrivate">Is This Book Public?</label>
    </div>
    <button type="submit" class="btn btn-outline-info">Submit</button>
  </form>


</template>


<style lang="scss" scoped></style>
