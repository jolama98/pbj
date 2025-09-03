<template>
  <div class="container-xxl py-4">
    <section class="row g-4">
      <div class="col-12">
        <div class="card border-0 shadow-sm gradient-border">
          <div class="card-body text-center">
            <h1 class="hero-title m-0">PBJ — a cozy home for poems</h1>
            <p class="text-secondary mt-2">Write, share, and save the words that move you.</p>
            <div class="d-flex gap-2 justify-content-center mt-3">
              <router-link class="btn btn-success rounded-4 text-light px-4" :to="{ name: 'Home' }">Browse
                Poems</router-link>
              <router-link class="btn btn-outline-info rounded-4 px-4" :to="{ name: 'Account' }">Create a
                Poem</router-link>
            </div>
          </div>
        </div>
      </div>

      <!-- Why + Features -->
      <div class="col-12 col-lg-7">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h2 class="section-title">Why PBJ</h2>
            <p>Most social feeds aren’t built for poetry. PBJ is a calm space to write, read, and collect poems—no
              noise, just words.</p>
            <h3 class="section-title mt-3">What you can do</h3>
            <ul class="mb-0">
              <li>Write & format poems, then like, comment, and save to <strong>Books</strong></li>
              <li>Use <strong>tags</strong> and <strong>genre</strong> for discovery</li>
              <li>Search across <em>title</em>, <em>body</em>, <em>tags</em>, and <em>genre</em></li>
              <li>Archive or delete any time—you own your work</li>
            </ul>
          </div>
        </div>
      </div>

      <!-- How + Community -->
      <div class="col-12 col-lg-5">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h2 class="section-title">How it works</h2>
            <ol class="mb-3">
              <li>Write a poem</li>
              <li>Add tags & a genre</li>
              <li>Share or save to a Book</li>
            </ol>
            <h3 class="section-title">Community</h3>
            <p class="mb-0 text-secondary small">Be kind. Credit authors. No harassment or hate. Keep PBJ cozy.</p>
          </div>
        </div>
      </div>

      <!-- Privacy & Roadmap -->
      <div class="col-12 col-lg-6">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h2 class="section-title">Privacy & Ownership</h2>
            <p>You own your poems. Export or delete them any time from your account.</p>
            <h3 class="section-title mt-3">Roadmap</h3>
            <ul class="mb-0">
              <li>Drafts & revision history</li>
              <li>Book covers & themes</li>
              <li>Better discovery & mobile polish</li>
            </ul>
          </div>
        </div>
      </div>

      <!-- About the creator -->
      <div class="col-12 col-lg-6">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h2 class="section-title">About the creator</h2>
            <p>Hi, I’m <strong>Joseph Hall</strong> — full-stack dev (C#/.NET, SQL, Vue.js), CodeWorks grad. I built PBJ
              because I love small, human places for writing.</p>
            <div class="d-flex flex-wrap gap-2 mt-2">
              <span class="chip">.NET</span><span class="chip">C#</span><span class="chip">MySQL</span>
              <span class="chip">Dapper</span><span class="chip">Vue.js</span><span class="chip">Auth0</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Contact -->
      <div class="col-12">
        <div class="card border-0 shadow-sm">
          <div class="card-body text-center">
            <h2 class="section-title">Say hi</h2>
            <p class="mb-3">Got feedback or ideas? I’d love to hear them.</p>
            <router-link class="btn btn-outline-info rounded-4 px-4" :to="{ name: 'Account' }">Send
              feedback</router-link>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped lang="scss">
.card {
  border-radius: 1rem;
  border: 1px solid rgba(0, 0, 0, .08);
  background: var(--bs-body-bg);
}

:root[data-bs-theme="dark"] .card {
  border-color: rgba(255, 255, 255, .08);
}

.gradient-border {
  border: 2px solid transparent;
  background: linear-gradient(var(--bs-body-bg), var(--bs-body-bg)) padding-box,
    linear-gradient(135deg, var(--bs-primary), var(--bs-info)) border-box;
}

.hero-title {
  font-weight: 900;
  letter-spacing: .2px;
  background: linear-gradient(135deg, var(--bs-primary), var(--bs-info));
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

.section-title {
  font-weight: 800;
  margin-bottom: .5rem;
}

.chip {
  display: inline-block;
  padding: .25rem .6rem;
  border-radius: 999px;
  border: 1px solid rgba(99, 102, 241, .25);
  background: rgba(99, 102, 241, .12);
  color: #6366f1;
  font-size: .9rem;
}

:root[data-bs-theme="dark"] .chip {
  border-color: rgba(99, 102, 241, .35);
  background: rgba(99, 102, 241, .16);
  color: #8b8efc;
}
</style>
