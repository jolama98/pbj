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

function setActivePoem(poemId) {
  try {
    poemsService.GetPoemById(poemId)
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
    await poemsService.deletePoem(props.poem.id)
    Pop.success("Poem Deleted!")
  }
  catch (error) {
    Pop.error(error);
  }
}

</script>


<template>
  <div v-if="poem" class="container-fluid">
    <div class="d-flex justify-content-end delete-btn">
      <i v-if="props.poem.authorId == account?.id" type="button" @click="deletePoem()"
        class="delete-button mdi mdi-close-octagon-outline fs-1 text-danger d-flex icon-pos">
      </i>
    </div>
    <div class="row">
      <div class="card">

        <div class="card-body" role="button" @click="setActivePoem(props.poem.id)">

          <h5 class="card-title">{{ props.poem.title }}</h5>
          <p class="card-text text-stop">{{ props.poem.body }}</p>
        </div>

        <div class="card-footer justify-content-end d-flex">


          <small class="text-body-secondary">
            <i class="mdi mdi-eye p-1"> {{ props.poem.views }}</i></small>
          <small class="text-body-secondary">
            <i class="mdi mdi-heart p-1">{{ props.poem.likes }}</i></small>
        </div>
        <!-- {{ props.poem.tags }} -->
      </div>
    </div>
  </div>
  <!-- </div> -->
</template>


<style lang="scss" scoped>
.delete-button {
  z-index: 1;
  position: relative;
  bottom: -50px;
  left: 8px;
}

.text-stop {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 4;
  overflow: hidden;
  white-space: pre;
  text-wrap: wrap;
}
</style>
