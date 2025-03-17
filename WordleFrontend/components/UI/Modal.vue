<template>
  <Transition
    enter-active-class="duration-300 ease-out"
    enter-from-class="opacity-0"
    enter-to-class="opacity-100"
    leave-active-class="duration-200 ease-in"
    leave-from-class="opacity-100"
    leave-to-class="opacity-0"
  >
    <div v-if="modelValue" class="fixed inset-0 z-50 overflow-y-auto">
      <div class="flex min-h-full items-center justify-center p-4">
        <!-- Backdrop with blur effect -->
        <div 
          class="fixed inset-0 bg-gradient-to-br from-black/30 via-purple-500/20 to-indigo-500/30 backdrop-blur-sm"
          @click="$emit('update:modelValue', false)"
        />
        
        <!-- Modal content with animations -->
        <Transition
          enter-active-class="duration-300 ease-out"
          enter-from-class="opacity-0 scale-95 translate-y-4"
          enter-to-class="opacity-100 scale-100 translate-y-0"
          leave-active-class="duration-200 ease-in"
          leave-from-class="opacity-100 scale-100 translate-y-0"
          leave-to-class="opacity-0 scale-95 translate-y-4"
        >
          <div class="relative w-full max-w-2xl transform overflow-hidden rounded-2xl shadow-[0_0_50px_rgba(99,102,241,0.3)] dark:shadow-[0_0_50px_rgba(99,102,241,0.15)]">
            <!-- Animated gradient border -->
            <div class="absolute inset-0 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 animate-border-flow">
              <div class="absolute inset-[2px] bg-white dark:bg-gray-800 rounded-2xl">
                <!-- Content -->
                <div class="p-6">
                  <slot></slot>
                </div>
              </div>
            </div>
          </div>
        </Transition>
      </div>
    </div>
  </Transition>
</template>

<script setup>
import { Transition } from 'vue'

defineProps({
  modelValue: {
    type: Boolean,
    required: true
  }
})

defineEmits(['update:modelValue'])
</script>

<style scoped>
@keyframes border-flow {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.animate-border-flow {
  animation: border-flow 4s linear infinite;
}

/* Add smooth transitions for all transformations */
.transform {
  transition-property: transform, opacity;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 300ms;
}

/* Hover effects for the modal */
.rounded-2xl:hover {
  transform: translateY(-2px);
}
</style> 