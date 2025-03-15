# راهنمای AI برای پروژه Wordle

<template>
  <div class="fixed inset-0 z-[70] flex items-center justify-center">
    <div class="absolute inset-0 bg-black/30 backdrop-blur-sm"></div>
    <div class="confetti-container">
      <div v-for="i in 50" :key="i" 
           class="confetti"
           :style="{
             '--delay': `${Math.random() * 3}s`,
             '--rotation': `${Math.random() * 360}deg`,
             '--rotation-end': `${Math.random() * 360 + 720}deg`,
             '--position-x': `${Math.random() * 100}%`,
             '--color': `hsl(${Math.random() * 360}, 70%, 50%)`
           }">
      </div>
    </div>
    <div class="win-message relative z-10 text-center p-8 rounded-2xl shadow-2xl">
      <div class="bg-gradient-to-br from-green-500/20 to-blue-500/20 backdrop-blur-md p-8 rounded-2xl border border-white/10">
        <h2 class="text-4xl font-bold text-white mb-4">{{ t('congratulations') }} 🎉</h2>
        <p class="text-xl text-white/90 mb-6">{{ t('youWon') }}</p>
        
        <!-- آمار مختصر -->
        <div class="grid grid-cols-2 gap-4 mb-6">
          <div class="text-center p-3 bg-white/10 rounded-lg">
            <p class="text-2xl font-bold text-white">{{ score }}</p>
            <p class="text-sm text-white/80">{{ t('score') }}</p>
          </div>
          <div class="text-center p-3 bg-white/10 rounded-lg">
            <p class="text-2xl font-bold text-white">{{ currentStreak }}</p>
            <p class="text-sm text-white/80">{{ t('currentStreak') }}</p>
          </div>
        </div>
        
        <button @click="$emit('playAgain')" 
                class="bg-gradient-to-r from-green-500/30 to-blue-500/30 hover:from-green-500/40 hover:to-blue-500/40 text-white px-8 py-3 rounded-lg text-lg font-semibold transition-all duration-300 hover:scale-105 border border-white/20">
          {{ t('playAgain') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
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

const t = (key) => {
  const translations = {
    congratulations: 'تبریک',
    youWon: 'شما برنده شدید!',
    playAgain: 'بازی مجدد',
    score: 'امتیاز',
    currentStreak: 'برد متوالی'
  }
  return translations[key]
}
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