<template>
  <div class="bg-white dark:bg-gray-800 rounded-xl p-8 max-w-md w-full mx-4 transform transition-all duration-300">
    <div class="flex justify-between items-center mb-6">
      <h3 class="text-2xl font-bold text-gray-900 dark:text-white font-display">{{ t('statistics') }}</h3>
      <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
        <i class="fas fa-times text-xl"></i>
      </button>
    </div>
    
    <div class="grid grid-cols-2 gap-4 mb-6">
      <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg hover:shadow-lg transition-all duration-300">
        <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ gamesPlayed }}</p>
        <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('gamesPlayed') }}</p>
      </div>
      <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg hover:shadow-lg transition-all duration-300">
        <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ winRate }}%</p>
        <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('winRate') }}</p>
      </div>
      <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg hover:shadow-lg transition-all duration-300">
        <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ currentStreak }}</p>
        <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('currentStreak') }}</p>
      </div>
      <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg hover:shadow-lg transition-all duration-300">
        <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ bestStreak }}</p>
        <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('bestStreak') }}</p>
      </div>
    </div>

    <div v-if="showPlayAgain" class="text-center">
      <button
        @click="$emit('playAgain')"
        class="bg-indigo-600 text-white px-6 py-3 rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200 shadow-lg"
      >
        {{ t('playAgain') }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { useTranslations } from '~/composables/useTranslations'

const { t } = useTranslations()

defineProps({
  gamesPlayed: {
    type: Number,
    default: 0
  },
  winRate: {
    type: Number,
    default: 0
  },
  currentStreak: {
    type: Number,
    default: 0
  },
  bestStreak: {
    type: Number,
    default: 0
  },
  showPlayAgain: {
    type: Boolean,
    default: false
  }
})

defineEmits(['close', 'playAgain'])
</script>

<style scoped>
.grid > div {
  position: relative;
  overflow: hidden;
}

.grid > div::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(45deg, transparent, rgba(255,255,255,0.1), transparent);
  transform: translateX(-100%);
  transition: transform 0.6s;
}

.grid > div:hover::before {
  transform: translateX(100%);
}
</style> 