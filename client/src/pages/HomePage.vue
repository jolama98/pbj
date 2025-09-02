<script setup>
import { AppState } from '@/AppState.js'
import PoemCard from '@/components/PoemCard.vue'
import ProfileCard from '@/components/ProfileCard.vue'
import { poemsService } from '@/services/PoemsService.js'
import Pop from '@/utils/Pop.js'
import { computed, onMounted, watch } from 'vue'
import ModalWrapper from '@/components/ModalWrapper.vue'
import PoemModal from '@/components/PoemModal.vue'

const poems = computed(() => AppState.poems)

watch(() => AppState.poemById, () => {
  getAllPoems()
})

onMounted(() => {
  getAllPoems()
})

async function getAllPoems() {
  try {
    await poemsService.getAllPoems()
  } catch (error) {
    Pop.error(error)
  }
}
</script>

<template>
  <div class="page">
    <div class="container-xxl py-3">
      <!-- Header / Breadcrumb area (optional) -->
      <header class="page__header">
        <h1 class="page__title">Discover Poems</h1>
        <p class="page__subtitle">Fresh verses from the community</p>
      </header>

      <section class="row g-4">
        <!-- Sidebar -->
        <aside class="col-lg-4 d-none d-lg-block">
          <div class="sidebar sticky-top">
            <ProfileCard />
          </div>
        </aside>

        <!-- Feed -->
        <main class="col-lg-8">
          <div class="feed card shadow-sm border-0">
            <div class="feed__scroll">
              <div v-for="poem in poems" :key="poem.id" class="feed__item">
                <PoemCard :poem="poem" />
              </div>

              <!-- Empty state -->
              <div v-if="!poems?.length" class="empty-state text-center py-5">
                <div class="empty-state__badge">No poems yet</div>
                <p class="mb-0">Be the first to share something beautiful.</p>
              </div>
            </div>
          </div>
        </main>
      </section>
    </div>
  </div>

  <ModalWrapper id="poem-modal">
    <PoemModal />
  </ModalWrapper>
</template>

<style scoped lang="scss">
/* Page shell */
.page {
  min-height: 100dvh;
  background:
    radial-gradient(1200px 600px at 10% -10%, rgba(111, 66, 193, .06), transparent 60%),
    radial-gradient(1200px 600px at 110% 10%, rgba(13, 110, 253, .06), transparent 60%),
    var(--bs-body-bg);
  color: var(--bs-body-color);
}

/* Header */
.page__header {
  margin-bottom: .5rem;

  .page__title {
    font-weight: 700;
    font-size: clamp(1.25rem, 1rem + 1.2vw, 1.75rem);
    margin: 0;
    letter-spacing: .2px;
  }

  .page__subtitle {
    margin: .25rem 0 0 0;
    opacity: .75;
    font-size: .95rem;
  }
}

/* Sidebar */
.sidebar {
  border-radius: 1rem;
  overflow: hidden;
  box-shadow: 0 10px 24px rgba(0, 0, 0, .06);
  top: 1rem;
  /* sticky offset */
}

/* Feed container */
.feed {
  border-radius: 1rem;
  background: var(--bs-body-bg);
  /* subtle glass effect */
  backdrop-filter: saturate(120%) blur(2px);
}

.feed__scroll {
  /* Full-height minus container padding and header height */
  max-height: calc(100dvh - 9.5rem);
  overflow: auto;
  padding: 1rem;
  scroll-behavior: smooth;
  scroll-padding: 1rem;

  /* Better scrollbars */
  &::-webkit-scrollbar {
    width: 10px;
  }

  &::-webkit-scrollbar-thumb {
    background: rgba(0, 0, 0, .15);
    border-radius: 999px;
  }

  &::-webkit-scrollbar-track {
    background: transparent;
  }
}

/* Each item block */
.feed__item {
  /* space between cards without relying on PoemCard internals */
  padding: .25rem;
  margin-bottom: 1rem;
  border-radius: .875rem;
  animation: fadeInUp .35s ease both;
  /* hover elevation */
  transition: transform .15s ease, box-shadow .15s ease, background-color .2s ease;
}

.feed__item:hover {
  transform: translateY(-1px);
  background: color-mix(in oklab, var(--bs-body-bg) 85%, var(--bs-primary) 15%, 8%);
  box-shadow: 0 8px 20px rgba(0, 0, 0, .06);
}

/* Empty state */
.empty-state__badge {
  display: inline-block;
  font-weight: 600;
  padding: .4rem .75rem;
  border-radius: 999px;
  background: rgba(13, 110, 253, .1);
  color: var(--bs-primary);
  margin-bottom: .5rem;
}

/* Animations */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(6px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Responsive tweaks */
@media (max-width: 991.98px) {
  .feed__scroll {
    max-height: none;
    /* let mobile page scroll normally */
  }
}

/* Optional: dark mode tuning if you’re not using Bootstrap’s variables */
:global(.dark) {
  .feed {
    background: rgba(23, 23, 23, .8);
  }

  .feed__item:hover {
    background: rgba(255, 255, 255, .03);
  }

  .sidebar {
    box-shadow: 0 10px 24px rgba(0, 0, 0, .3);
  }
}
</style>
