<script setup>
import { AppState } from '@/AppState.js';
import { Poem } from '@/models/Poem.js';
import { poemsService } from '@/services/PoemsService.js';
import Pop from '@/utils/Pop.js';
import { computed } from 'vue';
const account = computed(() => AppState.account)
const props = defineProps({
  poem: { type: Poem, required: true }
})

// function setActivePoem(poemId) {
//   try {
//     poemsService.GetPoemById(poemId)
//   }
//   catch (error) {
//     Pop.error(error);
//   }
// }

async function deletePoem() {
  try {
    const choice = await Pop.confirm("Are You Sure???", 'Delete Poem')
    if (choice == false) {
      Pop.toast("action canceled successfully", 'info', 'center')
      return
    }
    await poemsService.deletePoem(props.poem.id)
    Pop.success("Poem Deleted!")
  }
  catch (error) {
    Pop.error(error);
  }
}

</script>


<template>
  <div class="container">
    <div class="row">
      <div class="card flex-row">
        <!-- <img class="img-fluid" :src="poem.imgUrl" :alt="poem.title" :title="poem.title"> -->
        <!-- <div class="card-img-overlay d-flex align-items-end justify-content-between"> -->
        <div class="col-9">
          <div class="card-body">
            <p class="card-title fs-5">{{ poem.title }}</p>
            <p class="card-text text-stop"> sty{{ poem.body }}</p>
          </div>
        </div>
        <div class="col-3">
          <div class="d-flex justify-content-end">

            <i v-if="props.poem.authorId == account?.id" type="button" @click="deletePoem()"
              class="delete-button mdi mdi-close-octagon-outline fs-1 text-danger d-flex icon-pos  m-2">
            </i>
          </div>
        </div>

      </div>
    </div>
  </div>
  <!-- </div> -->
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
