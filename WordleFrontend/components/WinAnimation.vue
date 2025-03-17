# راهنمای AI برای پروژه Wordle

<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[70]">
    <div class="confetti-container">
      <div
        v-for="i in 50"
        :key="i"
        class="confetti"
        :style="{
          '--color': `hsl(${Math.random() * 360}deg, 100%, 50%)`,
          '--position-x': `${Math.random() * 100}%`,
          '--delay': `${Math.random() * 3}s`,
          '--rotation': `${Math.random() * 360}deg`,
          '--rotation-end': `${Math.random() * 360}deg`
        }"
      ></div>
    </div>
    <div class="bg-white dark:bg-gray-800 rounded-xl p-8 max-w-md w-full mx-4 transform transition-all duration-300 win-message">
      <div class="text-center">
        <div class="mb-6">
          <i class="fas fa-trophy text-6xl text-yellow-500 animate-bounce"></i>
        </div>
        <h2 class="text-3xl font-bold text-gray-900 dark:text-white mb-4 font-display">{{ t('congratulations') }}! 🎉</h2>
        <p class="text-lg text-gray-600 dark:text-gray-300 mb-6">{{ t('youWon') }}</p>
        
        <div class="grid grid-cols-2 gap-4 mb-8">
          <div class="bg-gradient-to-br from-indigo-100 to-purple-100 dark:from-indigo-900 dark:to-purple-900 p-4 rounded-lg">
            <p class="text-2xl font-bold text-indigo-700 dark:text-indigo-300">{{ score }}</p>
            <p class="text-sm text-indigo-600 dark:text-indigo-400">{{ t('score') }}</p>
          </div>
          <div class="bg-gradient-to-br from-green-100 to-teal-100 dark:from-green-900 dark:to-teal-900 p-4 rounded-lg">
            <p class="text-2xl font-bold text-green-700 dark:text-green-300">{{ currentStreak }}</p>
            <p class="text-sm text-green-600 dark:text-green-400">{{ t('currentStreak') }}</p>
          </div>
        </div>

        <button
          @click="$emit('playAgain')"
          class="bg-indigo-600 text-white px-8 py-3 rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200 shadow-lg"
        >
          {{ t('playAgain') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useTranslations } from '~/composables/useTranslations'

const { t } = useTranslations()

defineProps({
  score: {
    type: Number,
    required: true
  },
  currentStreak: {
    type: Number,
    required: true
  }
})

defineEmits(['playAgain'])
</script>

<style scoped>
.confetti-container {
  position: fixed;
  inset: 0;
  pointer-events: none;
  z-index: 1;
  overflow: hidden;
}

.confetti {
  position: absolute;
  width: 10px;
  height: 20px;
  background: var(--color);
  top: -20px;
  left: var(--position-x);
  opacity: 0;
  transform: rotate(var(--rotation));
  animation: confetti 3s var(--delay) ease-in-out infinite;
}

.win-message {
  animation: messageIn 0.5s ease-out 0.3s both;
}

@keyframes confetti {
  0% {
    opacity: 1;
    transform: translateY(0) rotate(var(--rotation));
  }
  50% {
    opacity: 1;
    transform: translateY(50vh) rotate(var(--rotation-end));
  }
  100% {
    opacity: 0;
    transform: translateY(100vh) rotate(var(--rotation-end));
  }
}

@keyframes messageIn {
  from {
    opacity: 0;
    transform: scale(0.8) translateY(20px);
  }
  to {
    opacity: 1;
    transform: scale(1) translateY(0);
  }
}
</style> 