<template>
  <Teleport to="body">
    <div v-show="modelValue" class="fixed inset-0 z-[9999] overflow-y-auto">
      <div class="flex min-h-full items-center justify-center p-4">
        <!-- Simple backdrop -->
        <div 
          class="fixed inset-0 bg-black/50"
          @click="$emit('update:modelValue', false)"
        />
        
        <!-- Simple modal content -->
        <div class="relative w-full max-w-2xl bg-white dark:bg-gray-800 rounded-lg shadow-xl z-[10000]">
          <div class="p-6">
            <slot></slot>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { Teleport } from 'vue'

defineProps({
  modelValue: {
    type: Boolean,
    required: true
  }
})

defineEmits(['update:modelValue'])
</script>

<style scoped>
.fixed {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
}
</style> 