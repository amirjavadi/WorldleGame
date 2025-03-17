# راهنمای AI برای پروژه Wordle

<template>
  <div class="flex items-center space-x-4 rtl:space-x-reverse">
    <div class="time-block">
      <div class="time-value">{{ hours }}</div>
      <div class="time-label">ساعت</div>
    </div>
    <div class="time-separator">:</div>
    <div class="time-block">
      <div class="time-value">{{ minutes }}</div>
      <div class="time-label">دقیقه</div>
    </div>
    <div class="time-separator">:</div>
    <div class="time-block">
      <div class="time-value">{{ seconds }}</div>
      <div class="time-label">ثانیه</div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  remainingTime: {
    type: Number,
    required: true
  }
})

const hours = computed(() => {
  return Math.floor(props.remainingTime / (1000 * 60 * 60)).toString().padStart(2, '0')
})

const minutes = computed(() => {
  return Math.floor((props.remainingTime % (1000 * 60 * 60)) / (1000 * 60)).toString().padStart(2, '0')
})

const seconds = computed(() => {
  return Math.floor((props.remainingTime % (1000 * 60)) / 1000).toString().padStart(2, '0')
})
</script>

<style scoped>
.time-block {
  @apply bg-gradient-to-br from-indigo-100 to-purple-100 dark:from-indigo-900 dark:to-purple-900 
         rounded-lg p-2 text-center min-w-[60px] transform transition-all duration-300;
}

.time-block:hover {
  @apply scale-105 shadow-lg;
}

.time-value {
  @apply text-2xl font-bold text-indigo-700 dark:text-indigo-300;
}

.time-label {
  @apply text-xs text-indigo-600 dark:text-indigo-400;
}

.time-separator {
  @apply text-2xl font-bold text-gray-400 dark:text-gray-500 animate-pulse;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0.5;
  }
}
</style> 