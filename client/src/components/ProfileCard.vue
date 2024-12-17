<script setup>

</script>


<template>
  <div class="container">
    <div class="card">
      <div class="card-title d-flex justify-content-center">
        <h1>Me</h1>
      </div>
      <div class="card-body d-flex flex-column">
        <div class="d-flex justify-content-center">
          <img class="avatar img-thumbnail mb-3 "
            src="https://images.unsplash.com/photo-1525355198643-193cd25842ae?w=800&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTJ8fGxlZ298ZW58MHx8MHx8fDA%3D"
            alt="">
        </div>
        <div class="flex-column d-flex">
          <button class="btn btn-danger mb-2">All</button>
          <button class="btn btn-danger mb-2">Fun</button>
          <button class="btn btn-danger mb-2">Stuff</button>
        </div>
      </div>
    </div>
  </div>
</template>


<style lang="scss" scoped>
.avatar {
  vertical-align: middle;
  width: 10vh;
  height: 10vh;
  border-radius: 50%;
}
</style>
