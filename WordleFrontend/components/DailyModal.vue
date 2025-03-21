<template>
  <div v-if="show" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white dark:bg-gray-800 rounded-lg p-6 max-w-md w-full mx-4">
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-xl font-bold">{{ t('dailyChallenge') }}</h2>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">
          <span class="sr-only">{{ t('close') }}</span>
          <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <div class="mb-6">
        <p class="text-gray-600 dark:text-gray-300 mb-2">{{ t('timeUntilNextChallenge') }}</p>
        <div class="flex justify-center space-x-4">
          <div class="text-center">
            <div class="text-2xl font-bold">{{ hours }}</div>
            <div class="text-sm text-gray-500">{{ t('hours') }}</div>
          </div>
          <div class="text-center">
            <div class="text-2xl font-bold">{{ minutes }}</div>
            <div class="text-sm text-gray-500">{{ t('minutes') }}</div>
          </div>
          <div class="text-center">
            <div class="text-2xl font-bold">{{ seconds }}</div>
            <div class="text-sm text-gray-500">{{ t('seconds') }}</div>
          </div>
        </div>
      </div>

      <div class="mb-6">
        <h3 class="text-lg font-semibold mb-2">{{ t('dailyChallengeRules') }}</h3>
        <ul class="list-disc list-inside space-y-2 text-gray-600 dark:text-gray-300">
          <li>{{ t('dailyChallengeRule1') }}</li>
          <li>{{ t('dailyChallengeRule2') }}</li>
          <li>{{ t('dailyChallengeRule3') }}</li>
        </ul>
      </div>

      <div class="flex justify-end">
        <button
          @click="$emit('start')"
          class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors"
        >
          {{ t('startChallenge') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import { useTranslations } from '../composables/useTranslations'

const props = defineProps({
  show: {
    type: Boolean,
    required: true
  }
})

const emit = defineEmits(['close', 'start'])

const { t } = useTranslations()

const hours = ref(0)
const minutes = ref(0)
const seconds = ref(0)

let timer = null

const updateTimer = () => {
  const now = new Date()
  const tomorrow = new Date(now)
  tomorrow.setDate(tomorrow.getDate() + 1)
  tomorrow.setHours(0, 0, 0, 0)
  
  const diff = tomorrow - now
  
  hours.value = Math.floor(diff / (1000 * 60 * 60))
  minutes.value = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60))
  seconds.value = Math.floor((diff % (1000 * 60)) / 1000)
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
</script> 