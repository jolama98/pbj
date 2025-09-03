<script setup>
import { AppState } from '@/AppState.js';
import { Poem } from '@/models/Poem.js';
import { likedPoemService } from '@/services/LikedPoemService.js';
import { poemsService } from '@/services/PoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';

const account = computed(() => AppState.account)

const props = defineProps({
  poem: { type: Poem, required: true }
})

const isOwner = computed(() => props.poem?.authorId == account.value?.id)
const liking = ref(false)

async function likePoem() {
  if (liking.value) return
  try {
    liking.value = true
    const poemId = { poemId: props.poem.id }
    await likedPoemService.createLikePoem(poemId)
  }
  catch (error) {
    Pop.error('Could not like poem', 'error')
    logger.error(error)
  }
  finally {
    liking.value = false
  }
}

async function setActivePoem(poemId) {
  try {
    await poemsService.getPoemById(poemId)
  }
  catch (error) {
    Pop.error(error)
  }
}

async function deletePoem() {
  try {
    const choice = await Pop.confirm('Are You Sure???', 'Delete Poem')
    if (!choice) {
      Pop.toast('Action canceled', 'info', 'center')
      return
    }
    Modal.getOrCreateInstance('#poem-modal').hide()
    await poemsService.deletePoem(props.poem.id)
    Pop.success('Poem Deleted!')
  }
  catch (error) {
    Pop.error(error)
  }
}
</script>

<template>
  <div class="poem-card card border-0 shadow-sm">
    <div v-if="poem" class="container-fluid p-0">
      <!-- Clickable body opens modal -->
      <div class="card-body cursor-pointer" data-bs-toggle="modal" data-bs-target="#poem-modal" role="button"
        @click="setActivePoem(props.poem.id)">
        <div class="d-flex align-items-start justify-content-between gap-3">
          <div class="pe-2">
            <h5 class="card-title mb-1 text-truncate" :title="props.poem.title">
              {{ props.poem.title }}
            </h5>
            <p class="card-subtitle small text-secondary mb-2">
              <!-- you can swap in author/display name here if you have it -->
              <span class="badge rounded-pill bg-gradient-subtle">Poem</span>
            </p>
          </div>

          <button v-if="isOwner" class="btn btn-link p-0 text-danger delete-btn" aria-label="Delete poem"
            title="Delete poem" @click.stop="deletePoem()">
            <i class="mdi mdi-close-octagon-outline fs-4"></i>
          </button>
        </div>

        <p class="card-text text-stop mb-0">{{ props.poem.body }}</p>
      </div>

      <!-- Footer actions (not part of modal trigger) -->
      <div class="card-footer d-flex align-items-center justify-content-between gap-2">
        <div class="d-flex align-items-center gap-3 text-secondary small">
          <span class="d-inline-flex align-items-center">
            <i class="mdi mdi-eye me-1"></i>{{ props.poem.views }}
          </span>
          <span class="d-inline-flex align-items-center">
            <i class="mdi mdi-sack me-1"></i>{{ props.poem.saves }}
          </span>
        </div>

        <button class="btn btn-like d-inline-flex align-items-center" :disabled="liking" @click.stop="likePoem()"
          aria-label="Like poem" title="Like">
          <i class="mdi mdi-heart me-1"></i>
          <span class="d-none d-sm-inline">Like</span>
          <span v-if="liking" class="spinner-border spinner-border-sm ms-2" role="status" aria-hidden="true"></span>
        </button>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
/* —— Theme-aware tokens (works with Bootstrap data-bs-theme) —— */
:root,
:root[data-bs-theme="light"] {
  --pc-bg: #ffffff;
  --pc-border: rgba(0, 0, 0, .06);
  --pc-hover: rgba(0, 0, 0, .05);
  --pc-title: #111827;
  /* gray-900 */
  --pc-muted: #6b7280;
  /* gray-500 */
  --pc-like-bg: #f3f4f6;
  /* gray-100 */
}

:root[data-bs-theme="dark"] {
  --pc-bg: #0f1115;
  --pc-border: rgba(255, 255, 255, .08);
  --pc-hover: rgba(255, 255, 255, .06);
  --pc-title: #e5e7eb;
  /* gray-200 */
  --pc-muted: #9ca3af;
  /* gray-400 */
  --pc-like-bg: #1f2937;
  /* gray-800 */
}

.poem-card {
  background: var(--pc-bg);
  border-radius: 1rem;
  overflow: clip;
  transition: transform .12s ease, box-shadow .12s ease, border-color .12s ease;
  border: 1px solid var(--pc-border);

  &:hover {
    transform: translateY(-2px);
    box-shadow: 0 .5rem 1.25rem rgba(0, 0, 0, .10);
    border-color: var(--pc-hover);
  }

  .card-body {
    padding: 1rem 1rem .75rem 1rem;

    @media (min-width: 576px) {
      padding: 1.25rem 1.25rem .75rem 1.25rem;
    }
  }

  .card-title {
    color: var(--pc-title);
    font-weight: 700;
    letter-spacing: .2px;
    max-width: 38ch;
  }

  .card-subtitle {
    color: var(--pc-muted);
  }

  .card-footer {
    background: transparent;
    border-top: 1px solid var(--pc-border);
    padding: .5rem .75rem;

    @media (min-width: 576px) {
      padding: .75rem 1rem;
    }
  }
}

/* Subtle “type” chip */
.bg-gradient-subtle {
  background: linear-gradient(180deg, rgba(99, 102, 241, .12), rgba(99, 102, 241, .06));
  color: #6366f1;
  border: 1px solid rgba(99, 102, 241, .25);
}

/* Truncate multi-line body nicely; keep user’s pre + wrapping intent */
.text-stop {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 4;
  /* show ~4 lines */
  overflow: hidden;
  white-space: pre-wrap;
  word-wrap: break-word;
  line-height: 1.45;
  color: var(--pc-title);
  opacity: .92;
}

/* Delete icon as a minimal link-style button */
.delete-btn {
  opacity: .85;
  transition: opacity .12s ease, transform .12s ease;

  &:hover {
    opacity: 1;
    transform: scale(1.05);
  }
}

/* Like button */
.btn-like {
  border: 1px solid var(--pc-border);
  background: var(--pc-like-bg);
  padding: .35rem .65rem;
  border-radius: .75rem;
  font-weight: 600;
  line-height: 1;

  i {
    font-size: 1.05rem;
  }

  &:hover {
    filter: brightness(1.05);
  }

  &:disabled {
    opacity: .75;
    cursor: not-allowed;
  }
}

/* Make whole card body feel clickable without affecting footer/buttons */
.cursor-pointer {
  cursor: pointer;
}
</style>
