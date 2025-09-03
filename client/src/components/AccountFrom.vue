<script setup>
import { ref, onMounted } from 'vue';
import { accountService } from '../services/AccountService.js';
import Pop from '../utils/Pop.js';
import { AppState } from '@/AppState.js';
import { logger } from '@/utils/Logger.js';

const editableAccountData = ref({
  name: '',
  picture: '',
  coverImg: '',
  email: ''
})

const defaultAvatar = 'https://placehold.co/96x96?text=%F0%9F%91%A4'
const defaultCover = 'https://placehold.co/1200x300?text=Cover'

onMounted(() => {
  editableAccountData.value = { ...AppState.account }
})

async function updateAccount() {
  try {
    await accountService.updateAccount(editableAccountData.value)
    Pop.success("Changes saved to your account!")
  }
  catch (error) {
    Pop.error(error)
    logger.log(error)
  }
}
</script>

<template>
  <div class="container-xxl py-4">
    <section class="row g-4 align-items-start">
      <!-- Header -->
      <div class="col-12">
        <header class="page-header d-flex justify-content-between align-items-end flex-wrap gap-2">
          <div>
            <h1 class="page-title m-0">Update your profile</h1>
            <p class="page-subtitle m-0 text-secondary">Make it shine — changes preview live.</p>
          </div>
        </header>
      </div>

      <!-- Live preview -->
      <div class="col-12 col-lg-5">
        <div class="card profile-card border-0 shadow-sm">
          <div class="profile-cover"
            :style="{ backgroundImage: `url(${editableAccountData.coverImg || defaultCover})` }" role="img"
            aria-label="Profile cover preview" />
          <div class="card-body d-flex align-items-center gap-3">
            <img class="avatar-lg" :src="editableAccountData.picture || defaultAvatar" alt="Profile avatar preview" />
            <div class="min-w-0">
              <h3 class="mb-0 text-truncate">{{ editableAccountData.name || 'Your Name' }}</h3>
              <small class="text-secondary text-truncate d-block">{{ editableAccountData.email || 'email@example.com'
                }}</small>
            </div>
          </div>
        </div>
      </div>

      <!-- Form -->
      <div class="col-12 col-lg-7">
        <div class="card form-card border-0 shadow-sm">
          <div class="card-body">
            <form @submit.prevent="updateAccount()">
              <div class="mb-3">
                <label for="name" class="form-label">Account Name</label>
                <input v-model="editableAccountData.name" type="text" class="form-control fancy-input" id="name"
                  required maxlength="100" placeholder="e.g., Joseph Hall">
              </div>

              <div class="mb-3">
                <label for="picture" class="form-label">Account Picture (URL)</label>
                <input v-model="editableAccountData.picture" type="url" class="form-control fancy-input" id="picture"
                  required maxlength="500" placeholder="https://…/avatar.png">
                <div class="form-text">Tip: Use a square image for best results.</div>
              </div>

              <div class="mb-3">
                <label for="coverImg" class="form-label">Cover Image (URL)</label>
                <input v-model="editableAccountData.coverImg" type="url" class="form-control fancy-input" id="coverImg"
                  maxlength="500" placeholder="https://…/cover.jpg">
              </div>

              <div class="mb-3">
                <label for="email" class="form-label">Email</label>
                <input v-model="editableAccountData.email" type="email" class="form-control fancy-input" id="email"
                  required maxlength="100" placeholder="you@example.com">
              </div>

              <div class="d-flex gap-2">
                <button type="submit" class="btn btn-success rounded-4 text-light px-4">Save changes</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style lang="scss" scoped>
:root,
:root[data-bs-theme="light"] {
  --page-title: #111827;
  /* gray-900 */
  --page-muted: #6b7280;
  /* gray-500 */
  --card-bg: #ffffff;
  --card-brd: rgba(0, 0, 0, .08);
  --chip-bg: rgba(99, 102, 241, .12);
  --chip-brd: rgba(99, 102, 241, .25);
}

:root[data-bs-theme="dark"] {
  --page-title: #e5e7eb;
  /* gray-200 */
  --page-muted: #9ca3af;
  /* gray-400 */
  --card-bg: #0f1115;
  --card-brd: rgba(255, 255, 255, .09);
  --chip-bg: rgba(99, 102, 241, .16);
  --chip-brd: rgba(99, 102, 241, .35);
}

/* Header */
.page-title {
  font-weight: 800;
  letter-spacing: .2px;
  color: var(--page-title);
  background: linear-gradient(135deg, var(--bs-primary), var(--bs-info));
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

.page-subtitle {
  color: var(--page-muted);
}

/* Cards */
.card {
  background: var(--card-bg);
  border: 1px solid var(--card-brd);
  border-radius: 1rem;
}

/* Profile preview */
.profile-card {
  overflow: hidden;
}

.profile-cover {
  height: 168px;
  background-size: cover;
  background-position: center;
  position: relative;
}

.profile-cover::after {
  /* soft overlay for readability */
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(180deg, rgba(0, 0, 0, .0), rgba(0, 0, 0, .10));
}

.avatar-lg {
  width: 88px;
  height: 88px;
  border-radius: 999px;
  object-fit: cover;
  border: 3px solid var(--card-bg);
  box-shadow: 0 8px 24px rgba(0, 0, 0, .15);
}

/* Form card + inputs (glass + gradient border like our search) */
.form-card .card-body {
  padding: 1.25rem 1.25rem;
}

.fancy-input {
  height: 46px;
  border-radius: .9rem;
  border: 2px solid transparent !important;
  background:
    linear-gradient(color-mix(in oklab, var(--bs-body-bg) 85%, transparent),
      color-mix(in oklab, var(--bs-body-bg) 85%, transparent)) padding-box,
    linear-gradient(135deg,
      color-mix(in oklab, var(--bs-primary) 35%, transparent),
      color-mix(in oklab, var(--bs-info) 35%, transparent)) border-box !important;
  backdrop-filter: saturate(120%) blur(6px);
  box-shadow: inset 0 1px 0 rgba(255, 255, 255, .05);
  transition: box-shadow .2s ease, background .2s ease;
}

.fancy-input::placeholder {
  color: color-mix(in oklab, var(--bs-body-color) 45%, transparent);
}

.fancy-input:focus {
  box-shadow:
    0 10px 28px rgba(0, 0, 0, .12),
    inset 0 0 0 1px color-mix(in oklab, var(--bs-primary) 25%, transparent);
  background:
    linear-gradient(color-mix(in oklab, var(--bs-body-bg) 80%, transparent),
      color-mix(in oklab, var(--bs-body-bg) 80%, transparent)) padding-box,
    linear-gradient(135deg, var(--bs-primary), var(--bs-info)) border-box !important;
  outline: none;
}

/* Small help text color */
.form-text {
  color: var(--page-muted);
}

/* Responsive spacing */
@media (max-width: 991.98px) {
  .profile-cover {
    height: 140px;
  }

  .avatar-lg {
    width: 76px;
    height: 76px;
  }
}
</style>
