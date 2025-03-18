<template>
  <div class="flex flex-col items-center justify-center space-y-2">
    <div class="text-sm text-gray-500 dark:text-gray-400">{{ t('dailyChallengeNextIn') }}</div>
    <div class="flex items-center space-x-2 rtl:space-x-reverse">
      <div class="flex flex-col items-center">
        <div class="text-2xl font-bold text-gray-900 dark:text-white">{{ hours }}</div>
        <div class="text-xs text-gray-500 dark:text-gray-400">{{ t('hours') }}</div>
      </div>
      <div class="text-2xl font-bold text-gray-900 dark:text-white">:</div>
      <div class="flex flex-col items-center">
        <div class="text-2xl font-bold text-gray-900 dark:text-white">{{ minutes }}</div>
        <div class="text-xs text-gray-500 dark:text-gray-400">{{ t('minutes') }}</div>
      </div>
      <div class="text-2xl font-bold text-gray-900 dark:text-white">:</div>
      <div class="flex flex-col items-center">
        <div class="text-2xl font-bold text-gray-900 dark:text-white">{{ seconds }}</div>
        <div class="text-xs text-gray-500 dark:text-gray-400">{{ t('seconds') }}</div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import { useTranslations } from '~/composables/useTranslations'

const { t } = useTranslations()

const hours = ref('00')
const minutes = ref('00')
const seconds = ref('00')

let timer = null

const updateTimer = () => {
  const now = new Date()
  const tomorrow = new Date(now)
  tomorrow.setDate(tomorrow.getDate() + 1)
  tomorrow.setHours(0, 0, 0, 0)

  const diff = tomorrow - now

  hours.value = Math.floor(diff / (1000 * 60 * 60)).toString().padStart(2, '0')
  minutes.value = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60)).toString().padStart(2, '0')
  seconds.value = Math.floor((diff % (1000 * 60)) / 1000).toString().padStart(2, '0')

  if (diff <= 0) {
    clearInterval(timer)
    // Emit event to parent to refresh challenge
    emit('refresh')
  }
}

onMounted(() => {
  updateTimer()
  timer = setInterval(updateTimer, 1000)
})

onUnmounted(() => {
  if (timer) {
    clearInterval(timer)
  }
})

const emit = defineEmits(['refresh'])
</script>

<style scoped>
.animate-pulse {
  animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: .5;
  }
}
</style> 