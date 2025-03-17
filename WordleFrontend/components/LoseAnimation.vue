<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[70]">
    <div class="shatter-container">
      <div
        v-for="i in 20"
        :key="i"
        class="shard"
        :style="{
          '--translate-x': `${(Math.random() - 0.5) * 200}px`,
          '--translate-y': `${(Math.random() - 0.5) * 200}px`,
          '--delay': `${Math.random() * 0.5}s`,
          '--rotate': `${Math.random() * 360}deg`
        }"
      ></div>
    </div>
    <div class="bg-white dark:bg-gray-800 rounded-xl p-8 max-w-md w-full mx-4 transform transition-all duration-300 game-over-message">
      <div class="text-center">
        <div class="mb-6">
          <i class="fas fa-heart-crack text-6xl text-red-500 animate-pulse"></i>
        </div>
        <h2 class="text-3xl font-bold text-gray-900 dark:text-white mb-4 font-display">{{ t('gameOver') }}</h2>
        <p class="text-lg text-gray-600 dark:text-gray-300 mb-2">{{ t('correctWordWas') }}:</p>
        <p class="text-2xl font-bold text-indigo-600 dark:text-indigo-400 mb-6 font-display">{{ correctWord }}</p>
        
        <div class="grid grid-cols-2 gap-4 mb-8">
          <div class="bg-gradient-to-br from-blue-100 to-purple-100 dark:from-blue-900 dark:to-purple-900 p-4 rounded-lg">
            <p class="text-2xl font-bold text-blue-700 dark:text-blue-300">{{ gamesPlayed }}</p>
            <p class="text-sm text-blue-600 dark:text-blue-400">{{ t('gamesPlayed') }}</p>
          </div>
          <div class="bg-gradient-to-br from-purple-100 to-pink-100 dark:from-purple-900 dark:to-pink-900 p-4 rounded-lg">
            <p class="text-2xl font-bold text-purple-700 dark:text-purple-300">{{ winRate }}%</p>
            <p class="text-sm text-purple-600 dark:text-purple-400">{{ t('winRate') }}</p>
          </div>
        </div>

        <button
          @click="$emit('playAgain')"
          class="bg-indigo-600 text-white px-8 py-3 rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200 shadow-lg"
        >
          {{ t('tryAgain') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useTranslations } from '~/composables/useTranslations'

const { t } = useTranslations()

defineProps({
  correctWord: {
    type: String,
    required: true
  },
  gamesPlayed: {
    type: Number,
    required: true
  },
  winRate: {
    type: Number,
    required: true
  }
})

defineEmits(['playAgain'])
</script>

<style scoped>
.shatter-container {
  position: fixed;
  inset: 0;
  pointer-events: none;
  z-index: 1;
}

.shard {
  position: absolute;
  top: 50%;
  left: 50%;
  width: 30px;
  height: 30px;
  background: linear-gradient(135deg, #ef4444, #7c3aed);
  clip-path: polygon(50% 0%, 100% 50%, 50% 100%, 0% 50%);
  animation: shatter 1.5s ease-out forwards;
  animation-delay: var(--delay);
}

.game-over-message {
  animation: messageIn 0.5s ease-out 0.3s both;
}

@keyframes shatter {
  0% {
    opacity: 1;
    transform: translate(-50%, -50%) scale(1) rotate(0deg);
  }
  100% {
    opacity: 0;
    transform: translate(
      calc(-50% + var(--translate-x)),
      calc(-50% + var(--translate-y))
    ) scale(0.5) rotate(var(--rotate));
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