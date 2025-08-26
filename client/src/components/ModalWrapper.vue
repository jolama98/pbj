<script setup>
defineProps({ modalId: String })
</script>

 
<template>
  <div class="modal fade" :id="modalId" :aria-labelledby="`${modalId}Label`">
    <div class="modal-dialog modal-fullscreen-sm-down">
      <div class="modal-content">
        <div class="modal-body">
          <!-- NOTE slot allows you to open the Custom element -->
          <!-- <ModalWrapper> </ModalWrapper> -->
          <!-- and insert your own content from other components -->
          <slot>
            <!-- This can be filled in with whatever you want ;) -->
            <!-- The content here in the slot works as a default if the slot isn't used -->
          </slot>

        </div>
      </div>
    </div>
  </div>
</template>


<style lang="scss" scoped></style>
