<script setup>
import { AppState } from '@/AppState.js';
import { commentsService } from '@/services/CommentService.js';
import { savePoemsService } from '@/services/SavePoemsService.js';
import { logger } from '@/utils/Logger.js';
import Pop from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import { computed, ref } from 'vue';

const accountBook = computed(() => AppState.profileBooks)
const activePoem = computed(() => AppState.poemById)
const commentsForPoem = computed(() => AppState.comments || [])

// ---------- Helpers ----------
function timeAgo(iso) {
  if (!iso) return ''
  const now = Date.now()
  const then = new Date(iso).getTime()
  const diff = Math.max(0, Math.floor((now - then) / 1000)) // seconds
  if (diff < 60) return `${diff}s ago`
  const m = Math.floor(diff / 60)
  if (m < 60) return `${m}m ago`
  const h = Math.floor(m / 60)
  if (h < 24) return `${h}h ago`
  const d = Math.floor(h / 24)
  if (d < 7) return `${d}d ago`
  const w = Math.floor(d / 7)
  if (w < 5) return `${w}w ago`
  const months = Math.floor(d / 30)
  if (months < 12) return `${months}mo ago`
  const y = Math.floor(months / 12)
  return `${y}y ago`
}

// ---------- Comment form state ----------
const MAX_LEN = 160
const submittingComment = ref(false)
const commentData = ref({ body: '', poemId: 0 })
const charCount = computed(() => commentData.value.body.length)
const canSubmitComment = computed(() =>
  charCount.value > 0 && charCount.value <= MAX_LEN && !!activePoem.value?.id
)

async function comment() {
  if (submittingComment.value || !canSubmitComment.value) return
  try {
    submittingComment.value = true
    commentData.value.poemId = activePoem.value.id
    logger.log('Submitting comment', commentData.value)
    await commentsService.createComment(commentData.value, activePoem.value.id)
    commentData.value = { body: '', poemId: 0 }
    Pop.success('Comment posted!')
  } catch (error) {
    Pop.error(error)
  } finally {
    submittingComment.value = false
  }
}

// ---------- Save-to-book state ----------
const savedPoemToBookData = ref({ bookId: 0, poemId: 0 })
const saving = ref(false)
const canSave = computed(() => Number(savedPoemToBookData.value.bookId) > 0 && !!activePoem.value?.id)

async function createSavePoem() {
  if (saving.value || !canSave.value) return
  try {
    saving.value = true
    savedPoemToBookData.value.poemId = activePoem.value.id
    await savePoemsService.createSavePoem(savedPoemToBookData.value)
    logger.log('Saved poem to book', savedPoemToBookData.value)
    savedPoemToBookData.value = { bookId: 0, poemId: 0 }
    Modal.getOrCreateInstance('#poem-modal').hide()
    Pop.success('Poem added to book!')
  } catch (error) {
    Pop.error(error)
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <div class="row g-3 poem-modal" v-if="activePoem">
    <!-- Title + Tags -->
    <div class="col-12">
      <div class="card border-0 shadow-sm">
        <div class="card-header poem-title d-flex flex-wrap justify-content-between align-items-center gap-2">
          <h2 class="m-0 text-center flex-grow-1">{{ activePoem.title }}</h2>
          <span class="genre-chip ms-auto" :title="activePoem.tags || 'Poem'">
            <i class="mdi mdi-tag-multiple me-1"></i>{{ activePoem.tags || 'Poem' }}
          </span>
        </div>
      </div>
    </div>

    <!-- Body -->
    <div class="col-12">
      <div class="card border-0 shadow-sm">
        <div class="card-body">
          <p class="poem-body mb-0">{{ activePoem.body }}</p>
        </div>
      </div>
    </div>

    <!-- Comments list -->
    <div class="col-12">
      <div class="card border-0 shadow-sm">
        <!-- <div class="card-body py-3">
          <h6 class="mb-3 text-secondary">
            Comments • {{ commentsForPoem.length }}
          </h6>

          <div v-if="commentsForPoem.length === 0" class="empty-comments text-secondary small">
            No comments yet. Be the first to share a thought ✨
          </div>

          <ul v-else class="list-unstyled m-0">
            <li v-for="c in commentsForPoem" :key="c.id" class="comment-row d-flex gap-3 py-2">
              <img class="avatar"
                :src="c?.creator?.picture || c?.account?.picture || 'https://placehold.co/48x48?text=%F0%9F%93%9A'"
                alt="avatar" />
              <div class="flex-grow-1 min-w-0">
                <div class="d-flex justify-content-between align-items-baseline">
                  <div class="truncate">
                    <span class="fw-semibold me-2">{{ c?.creator?.name || c?.account?.name || 'Someone' }}</span>
                    <small class="text-secondary">{{ timeAgo(c?.createdAt) }}</small>
                  </div>
                </div>
                <div class="comment-body">
                  {{ c?.body }}
                </div>
              </div>
            </li>
          </ul>
        </div> -->

        <!-- Footer toolbar -->
        <div class="modal-footer flex-wrap gap-2 justify-content-between">
          <!-- Counters -->
          <div class="d-flex align-items-center gap-3 text-secondary small">
            <span class="d-inline-flex align-items-center">
              <i class="mdi mdi-eye me-1"></i> {{ activePoem.views }}
            </span>
            <span class="d-inline-flex align-items-center">
              <i class="mdi mdi-sack me-1"></i> {{ activePoem.saves }}
            </span>
          </div>

          <div class="d-flex align-items-center gap-2">
            <!-- Comment dropup -->
            <div class="dropup">
              <button class="btn btn-outline-primary btn-sm d-inline-flex align-items-center" data-bs-toggle="dropdown"
                aria-expanded="false" @click.stop>
                <i class="mdi mdi-comment-edit me-1"></i>
                Comment
              </button>
              <div class="dropdown-menu p-3 shadow-lg dropdown-form" @click.stop>
                <form @submit.prevent="comment()">
                  <label for="comment"
                    class="form-label fw-semibold d-flex justify-content-between align-items-center mb-1">
                    <span>Leave a comment</span>
                    <small :class="{ 'text-danger': charCount > MAX_LEN }">
                      {{ charCount }}/{{ MAX_LEN }}
                    </small>
                  </label>
                  <textarea id="comment" v-model="commentData.body" :maxlength="MAX_LEN" rows="3"
                    class="form-control mb-2" placeholder="Be kind ✨" required />
                  <button type="submit" class="btn btn-primary w-100"
                    :disabled="!canSubmitComment || submittingComment">
                    <span v-if="!submittingComment" class="d-inline-flex align-items-center">
                      <i class="mdi mdi-send me-1"></i> Post
                    </span>
                    <span v-else class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                  </button>
                </form>
              </div>
            </div>

            <!-- Save to book -->
            <form class="d-flex align-items-stretch save-form" @submit.prevent="createSavePoem()" @click.stop>
              <select v-model="savedPoemToBookData.bookId" class="form-select form-select-sm no-round-end"
                aria-label="Select a book to save to">
                <option :value="0">Select a Book</option>
                <option v-for="book in accountBook" :key="book.id" :value="book.id">
                  {{ book.title }}
                </option>
              </select>
              <button type="submit" class="btn btn-success btn-sm text-light no-round-start"
                :disabled="!canSave || saving" title="Save poem to book">
                <span v-if="!saving" class="d-inline-flex align-items-center">
                  <i class="mdi mdi-content-save me-1"></i> Save
                </span>
                <span v-else class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
              </button>
            </form>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<style lang="scss" scoped>
:root,
:root[data-bs-theme="light"] {
  --pm-bg: #ffffff;
  --pm-title: #111827;
  --pm-muted: #6b7280;
  --pm-border: rgba(0, 0, 0, .08);
  --pm-body: #1f2937;
  --pm-chip-bg: rgba(99, 102, 241, .12);
  --pm-chip-border: rgba(99, 102, 241, .25);
  --pm-chip-fg: #6366f1;
}

:root[data-bs-theme="dark"] {
  --pm-bg: #0f1115;
  --pm-title: #e5e7eb;
  --pm-muted: #9ca3af;
  --pm-border: rgba(255, 255, 255, .08);
  --pm-body: #e5e7eb;
  --pm-chip-bg: rgba(99, 102, 241, .16);
  --pm-chip-border: rgba(99, 102, 241, .35);
  --pm-chip-fg: #8b8efc;
}

.poem-modal .card {
  background: var(--pm-bg);
  border-radius: 1rem;
  border: 1px solid var(--pm-border);
}

.poem-title {
  color: var(--pm-title);
  border-bottom: 1px solid var(--pm-border);
  padding: .9rem 1.1rem;
  font-weight: 800;
  letter-spacing: .2px;
  display: flex;
  gap: .75rem;
}

.genre-chip {
  background: var(--pm-chip-bg);
  color: var(--pm-chip-fg);
  border: 1px solid var(--pm-chip-border);
  border-radius: 999px;
  padding: .25rem .6rem;
  font-size: .8rem;
  white-space: nowrap;
}

.card-body {
  padding: 1.1rem 1.25rem;
}

.poem-body {
  white-space: pre-wrap;
  word-wrap: break-word;
  color: var(--pm-body);
  line-height: 1.6;
  font-size: 1.05rem;
}

/* Comments */
.comment-row {
  border-top: 1px dashed var(--pm-border);
}

.comment-row:first-child {
  border-top: none;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 999px;
  object-fit: cover;
  flex: 0 0 40px;
  border: 1px solid var(--pm-border);
}

.comment-body {
  color: var(--pm-body);
  white-space: pre-wrap;
  word-break: break-word;
  font-size: .98rem;
}

.truncate {
  min-width: 0;
}

.truncate>* {
  vertical-align: middle;
}

/* Footer toolbar */
.modal-footer {
  border-top: 1px solid var(--pm-border);
  background: transparent;
  padding: .75rem 1rem;
}

/* Dropdown form panel */
.dropdown-form {
  min-width: 20rem;
  border: 1px solid var(--pm-border);
  border-radius: .75rem;
}

/* Save form combo */
.save-form .form-select {
  min-width: 12rem;
}

.no-round-start {
  border-top-left-radius: 0 !important;
  border-bottom-left-radius: 0 !important;
}

.no-round-end {
  border-top-right-radius: 0 !important;
  border-bottom-right-radius: 0 !important;
}

.btn-sm i {
  font-size: 1rem;
}
</style>
