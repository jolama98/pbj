<script setup>
import { computed } from 'vue';
import { AppState } from '../AppState.js';
import AccountFrom from '@/components/AccountFrom.vue';

const account = computed(() => AppState.account)

const defaultAvatar = 'https://placehold.co/160x160?text=%F0%9F%91%A4'
</script>

<template>
  <AccountFrom />

  <div class="container-xxl py-4">
    <!-- Loaded state -->
    <section v-if="account" class="row justify-content-center">
      <div class="col-12 col-md-10 col-lg-8">
        <div class="card profile-card border-0 shadow-sm">
          <div class="card-body d-flex align-items-center gap-3 gap-md-4">
            <img class="avatar-xl" :src="account.picture || defaultAvatar" :alt="account.name || 'Profile avatar'" />
            <div class="min-w-0">
              <h1 class="welcome-title text-truncate mb-1">
                Welcome {{ account.name }}
              </h1>
              <span class="email-chip text-truncate">{{ account.email }}</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Loading state -->
    <section v-else class="row justify-content-center">
      <div class="col-12 col-md-8">
        <div class="card border-0 shadow-sm glass-card">
          <div class="card-body py-5 text-center">
            <h1 class="m-0">Loading… <i class="mdi mdi-loading mdi-spin"></i></h1>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped lang="scss">
:root,
:root[data-bs-theme="light"] {
  --pg-title: #111827;
  /* gray-900 */
  --pg-muted: #6b7280;
  /* gray-500 */
  --card-bg: #ffffff;
  --card-brd: rgba(0, 0, 0, .08);
  --chip-bg: rgba(99, 102, 241, .12);
  --chip-brd: rgba(99, 102, 241, .25);
  --chip-fg: #6366f1;
}

:root[data-bs-theme="dark"] {
  --pg-title: #e5e7eb;
  /* gray-200 */
  --pg-muted: #9ca3af;
  /* gray-400 */
  --card-bg: #0f1115;
  --card-brd: rgba(255, 255, 255, .09);
  --chip-bg: rgba(99, 102, 241, .16);
  --chip-brd: rgba(99, 102, 241, .35);
  --chip-fg: #8b8efc;
}

/* Base card */
.card {
  background: var(--card-bg);
  border: 1px solid var(--card-brd);
  border-radius: 1rem;
}

/* Profile card with gradient border */
.profile-card {
  border: 2px solid transparent;
  background:
    linear-gradient(var(--card-bg), var(--card-bg)) padding-box,
    linear-gradient(135deg,
      color-mix(in oklab, var(--bs-primary) 35%, transparent),
      color-mix(in oklab, var(--bs-info) 35%, transparent)) border-box;
}

/* Subtle glass card for loading */
.glass-card {
  backdrop-filter: saturate(120%) blur(6px);
}

/* Avatar */
.avatar-xl {
  width: 96px;
  height: 96px;
  object-fit: cover;
  border-radius: 999px;
  border: 3px solid var(--card-bg);
  box-shadow: 0 8px 24px rgba(0, 0, 0, .12);
}

@media (min-width: 768px) {
  .avatar-xl {
    width: 112px;
    height: 112px;
  }
}

/* Title with gradient text */
.welcome-title {
  font-weight: 800;
  letter-spacing: .2px;
  margin: 0;
  background: linear-gradient(135deg, var(--bs-primary), var(--bs-info));
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

/* Email chip */
.email-chip {
  display: inline-block;
  margin-top: .25rem;
  padding: .25rem .6rem;
  font-size: .9rem;
  color: var(--chip-fg);
  background: var(--chip-bg);
  border: 1px solid var(--chip-brd);
  border-radius: 999px;
  max-width: 100%;
}

/* Legacy style override from original snippet (kept small for consistency) */
img {
  max-width: 100%;
}
</style>
