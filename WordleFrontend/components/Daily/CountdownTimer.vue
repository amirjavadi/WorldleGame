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
import { ref, onMounted, onUnmounted, computed, watch } from 'vue'
import { useTranslations } from '~/composables/useTranslations'
import { useDailyChallengeStore } from '~/stores/dailyChallenge'

const { t } = useTranslations()
const dailyStore = useDailyChallengeStore()

const timeUntilNext = computed(() => dailyStore.timeUntilNextChallenge)
const hours = computed(() => {
  if (!timeUntilNext.value) return '00'
  return String(Math.floor(timeUntilNext.value / (1000 * 60 * 60))).padStart(2, '0')
})
const minutes = computed(() => {
  if (!timeUntilNext.value) return '00'
  return String(Math.floor((timeUntilNext.value % (1000 * 60 * 60)) / (1000 * 60))).padStart(2, '0')
})
const seconds = computed(() => {
  if (!timeUntilNext.value) return '00'
  return String(Math.floor((timeUntilNext.value % (1000 * 60)) / 1000)).padStart(2, '0')
})

// تعریف emit برای رفرش
const emit = defineEmits(['refresh'])

// شروع تایمر در هنگام mount
onMounted(() => {
  if (timeUntilNext.value <= 0) {
    emit('refresh')
  }
})

// واکنش به تغییرات timeUntilNextChallenge
watch(() => timeUntilNext.value, (newValue) => {
  if (newValue <= 0) {
    emit('refresh')
  }
})
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