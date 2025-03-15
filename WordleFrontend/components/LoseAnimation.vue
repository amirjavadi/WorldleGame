<template>
  <div class="fixed inset-0 z-[70] flex items-center justify-center">
    <div class="absolute inset-0 bg-black/60 backdrop-blur-sm"></div>
    <div class="shatter-container">
      <div v-for="i in 20" :key="i" 
           class="shard"
           :style="{
             '--delay': `${Math.random() * 0.5}s`,
             '--rotate': `${Math.random() * 360}deg`,
             '--translate-x': `${(Math.random() - 0.5) * 100}vw`,
             '--translate-y': `${(Math.random() - 0.5) * 100}vh`,
           }">
      </div>
    </div>
    <div class="game-over-message relative z-10 text-center p-8 rounded-2xl shadow-2xl">
      <div class="bg-gradient-to-br from-red-500/20 to-purple-500/20 backdrop-blur-md p-8 rounded-2xl border border-white/10">
        <h2 class="text-4xl font-bold text-white mb-4">{{ t('gameOver') }} 💔</h2>
        <p class="text-xl text-white/90 mb-6">{{ t('correctWordWas') }}: <span class="font-bold text-2xl text-red-400">{{ correctWord }}</span></p>
        
        <!-- آمار مختصر -->
        <div class="grid grid-cols-2 gap-4 mb-6">
          <div class="text-center p-3 bg-white/10 rounded-lg">
            <p class="text-2xl font-bold text-white">{{ gamesPlayed }}</p>
            <p class="text-sm text-white/80">{{ t('gamesPlayed') }}</p>
          </div>
          <div class="text-center p-3 bg-white/10 rounded-lg">
            <p class="text-2xl font-bold text-white">{{ winRate }}%</p>
            <p class="text-sm text-white/80">{{ t('winRate') }}</p>
          </div>
        </div>
        
        <button @click="$emit('playAgain')" 
                class="bg-gradient-to-r from-red-500/30 to-purple-500/30 hover:from-red-500/40 hover:to-purple-500/40 text-white px-8 py-3 rounded-lg text-lg font-semibold transition-all duration-300 hover:scale-105 border border-white/20">
          {{ t('playAgain') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
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

const t = (key) => {
  const translations = {
    gameOver: 'بازی تمام شد',
    correctWordWas: 'کلمه درست',
    playAgain: 'بازی مجدد',
    gamesPlayed: 'تعداد بازی',
    winRate: 'درصد برد'
  }
  return translations[key]
}
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