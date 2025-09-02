<script setup>
import { AppState } from '@/AppState.js';
import { AuthService } from '@/services/AuthService.js';
import { computed } from 'vue';
import ModalWrapper from './ModalWrapper.vue';
import CreatePoemModal from './CreatePoemModal.vue';
import CreateBookModal from './CreateBookModal.vue';
import Pop from '@/utils/Pop.js';
import { poemsService } from '@/services/PoemsService.js';

// ---- state ----
const identity = computed(() => AppState.identity);
const account = computed(() => AppState.account);
const likedPoems = computed(() => AppState.likedPoem);
const books = computed(() => AppState.profileBooks);
const savedPoems = computed(() => AppState.savedPoem);

// If you render this component more than once on a page, change idBase via prop.
const idBase = 'profileSidebar'; // keep consistent, or accept as a prop

// ---- actions ----
async function login() { AuthService.loginWithPopup(); }
async function logout() { AuthService.logout(); }
async function getAllPoems() {
  try { await poemsService.getAllPoems(); }
  catch (error) { Pop.error(error); }
}
function loadSavedPoems() { console.log(savedPoems.value); }
</script>

<template>
  <div class="container py-3">
    <div class="card border-0 shadow-sm rounded-4 overflow-hidden">
      <!-- Header: vertical stack -->
      <div class="card-header bg-gradient py-4">
        <div class="d-flex flex-column align-items-center text-center">
          <div v-if="identity" class="dropdown position-static">
            <button class="p-0 bg-transparent border-0" data-bs-toggle="dropdown" data-bs-display="static"
              aria-expanded="false" aria-label="Open profile menu">
              <img class="avatar ring mb-2" :src="account?.picture || identity?.picture"
                :alt="account?.name || 'Profile'" />
            </button>
            <ul class="dropdown-menu dropdown-menu-end shadow-sm w-100">
              <li>
                <button class="dropdown-item text-danger d-flex align-items-center gap-2" @click="logout">
                  <i class="mdi mdi-logout"></i> Logout
                </button>
              </li>
            </ul>
          </div>

          <div v-else class="mb-2">
            <div class="avatar placeholder mb-2"></div>
          </div>

          <h1 class="h5 fw-semibold mb-1">{{ identity ? account?.name : 'Welcome' }}</h1>
          <p class="text-white-50 small mb-0">
            {{ identity ? 'Glad you’re here 👋' : 'Sign in to access your poems & books' }}
          </p>
        </div>
      </div>

      <!-- Body: pure vertical layout for a narrow col-3 -->
      <div class="card-body p-2 sidebar-vertical">
        <!-- MOBILE (<= md): Accordion keeps sections compact -->
        <div class="d-md-none">
          <div class="accordion" :id="`${idBase}-accordion`">
            <!-- Actions -->
            <div class="accordion-item">
              <h2 class="accordion-header" :id="`${idBase}-heading-actions`">
                <button class="accordion-button collapsed py-2" type="button" data-bs-toggle="collapse"
                  :data-bs-target="`#${idBase}-collapse-actions`" aria-expanded="false"
                  :aria-controls="`${idBase}-collapse-actions`">
                  Actions
                </button>
              </h2>
              <div class="accordion-collapse collapse" :id="`${idBase}-collapse-actions`"
                :aria-labelledby="`${idBase}-heading-actions`" :data-bs-parent="`#${idBase}-accordion`">
                <div class="accordion-body p-2">
                  <div v-if="identity" class="d-flex flex-column gap-2">
                    <button class="btn btn-info btn-sm w-100" data-bs-toggle="modal" data-bs-target="#create-poem">
                      <i class="mdi mdi-feather"></i> Create Poem
                    </button>

                    <!-- Optional:
                    <button class="btn btn-outline-info btn-sm w-100" data-bs-toggle="modal" data-bs-target="#create-book">
                      Create Book
                    </button> -->

                    <router-link :to="{ name: 'Account' }" class="btn btn-outline-info btn-sm w-100">
                      Edit Profile
                    </router-link>
                  </div>
                  <div v-else class="pt-1">
                    <button class="btn btn-success btn-sm w-100" @click="login">Login</button>
                  </div>
                </div>
              </div>
            </div>

            <!-- Stats -->
            <div class="accordion-item">
              <h2 class="accordion-header" :id="`${idBase}-heading-stats`">
                <button class="accordion-button collapsed py-2" type="button" data-bs-toggle="collapse"
                  :data-bs-target="`#${idBase}-collapse-stats`" aria-expanded="false"
                  :aria-controls="`${idBase}-collapse-stats`">
                  Stats
                </button>
              </h2>
              <div class="accordion-collapse collapse" :id="`${idBase}-collapse-stats`"
                :aria-labelledby="`${idBase}-heading-stats`" :data-bs-parent="`#${idBase}-accordion`">
                <div class="accordion-body p-2">
                  <div v-if="identity" class="d-flex flex-column gap-2">
                    <!-- <div class="stat-row">Books <span class="value">{{ books.length }}</span></div> -->
                    <div class="stat-row">Liked <span class="value">{{ likedPoems.length }}</span></div>
                    <button class="stat-row text-start" @click="loadSavedPoems">
                      Saved <span class="value">{{ savedPoems.length }}</span>
                    </button>
                  </div>
                  <div v-else class="text-muted small">Sign in to see stats</div>
                </div>
              </div>
            </div>

            <!-- Filter -->
            <div class="accordion-item">
              <h2 class="accordion-header" :id="`${idBase}-heading-filter`">
                <button class="accordion-button collapsed py-2" type="button" data-bs-toggle="collapse"
                  :data-bs-target="`#${idBase}-collapse-filter`" aria-expanded="false"
                  :aria-controls="`${idBase}-collapse-filter`">
                  Filter
                </button>
              </h2>
              <div class="accordion-collapse collapse" :id="`${idBase}-collapse-filter`"
                :aria-labelledby="`${idBase}-heading-filter`" :data-bs-parent="`#${idBase}-accordion`">
                <div class="accordion-body p-2">
                  <div class="btn-group w-100 position-static">
                    <button class="btn btn-outline-info btn-sm w-100" type="button" data-bs-toggle="dropdown"
                      data-bs-display="static" aria-expanded="false">
                      Choose Filter <span class="mdi mdi-chevron-down"></span>
                    </button>
                    <ul class="dropdown-menu dropdown-menu-end w-100">
                      <li><button class="dropdown-item">Newest</button></li>
                      <li><button class="dropdown-item">Most Liked</button></li>
                      <li><button class="dropdown-item">Saved</button></li>
                    </ul>
                  </div>
                  <button class="btn btn-outline-info btn-sm w-100 mt-2" @click="getAllPoems">All</button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- DESKTOP/TABLET (>= md): expanded but still vertical -->
        <div class="d-none d-md-flex flex-column gap-2">
          <div v-if="identity" class="d-flex flex-column gap-2">
            <button class="btn btn-info btn-sm w-100" data-bs-toggle="modal" data-bs-target="#create-poem">
              <i class="mdi mdi-feather"></i> Create Poem
            </button>

            <!-- Optional:
            <button class="btn btn-outline-info btn-sm w-100" data-bs-toggle="modal" data-bs-target="#create-book">
              Create Book
            </button> -->

            <router-link :to="{ name: 'Account' }" class="btn btn-outline-info btn-sm w-100">
              Edit Profile
            </router-link>
          </div>
          <div v-else>
            <button class="btn btn-success btn-sm w-100" @click="login">Login</button>
          </div>

          <hr class="my-2" />

          <div v-if="identity" class="d-flex flex-column gap-2">
            <!-- <div class="stat-row">Books <span class="value">{{ books.length }}</span></div> -->
            <div class="stat-row text-dark">Liked <span class="value">{{ likedPoems.length }}</span></div>
            <button class="stat-row text-start text-dark" @click="loadSavedPoems">
              Saved <span class="value">{{ savedPoems.length }}</span>
            </button>
          </div>

          <hr class="my-2" />

          <div class="d-flex flex-column gap-2">
            <div class="btn-group w-100 position-static">
              <button class="btn btn-outline-info btn-sm w-100" type="button" data-bs-toggle="dropdown"
                data-bs-display="static" aria-expanded="false">
                Filter <span class="mdi mdi-chevron-down"></span>
              </button>
              <ul class="dropdown-menu dropdown-menu-end w-100">
                <li><button class="dropdown-item">Newest</button></li>
                <li><button class="dropdown-item">Most Liked</button></li>
                <li><button class="dropdown-item">Saved</button></li>
              </ul>
            </div>
            <button class="btn btn-outline-info btn-sm w-100" @click="getAllPoems">All</button>
          </div>
        </div>
      </div>
    </div>
  </div>

  <ModalWrapper id="create-poem">
    <CreatePoemModal />
  </ModalWrapper>
  <ModalWrapper id="create-book">
    <CreateBookModal />
  </ModalWrapper>
</template>

<style lang="scss" scoped>
/* Header */
.bg-gradient {
  background: linear-gradient(135deg, #0dcaf0 0%, #0aa2c0 100%);
  color: #fff;
}

/* Avatar */
.avatar {
  width: 72px;
  height: 72px;
  border-radius: 50%;
  object-fit: cover;
}

.avatar.placeholder {
  width: 72px;
  height: 72px;
  border-radius: 50%;
  background: rgba(255, 255, 255, .35);
}

.ring {
  box-shadow:
    0 0 0 3px rgba(255, 255, 255, 0.85),
    0 6px 18px rgba(0, 0, 0, 0.15);
  transition: transform .15s ease;
}

.ring:hover {
  transform: translateY(-1px) scale(1.01);
}

/* Vertical sidebar helpers */
.sidebar-vertical .btn {
  white-space: nowrap;
  /* remove if you prefer wrapping labels */
}

.stat-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: .5rem;
  width: 100%;
  padding: .45rem .6rem;
  border: 1px solid #e9eef3;
  border-radius: .5rem;
  background: #f8fafc;
  font-size: .9rem;
}

.stat-row .value {
  font-weight: 700;
}

/* Compact accordion headers for a sidebar */
.accordion-button {
  font-size: .95rem;
}

/* Ensure dropdowns behave inside narrow columns */
.dropdown-menu {
  max-width: 100%;
}
</style>

}