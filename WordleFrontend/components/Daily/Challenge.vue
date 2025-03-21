<template>
  <div class="flex flex-col items-center justify-center space-y-8">
    <!-- Header -->
    <div class="text-center">
      <h2 class="text-3xl font-bold text-gray-900 dark:text-white font-display">{{ t('dailyChallenge') }}</h2>
      <p class="text-gray-500 dark:text-gray-400 mt-2">{{ t('dailyChallengeDescription') }}</p>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="text-center py-8">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="text-center py-8">
      <i class="fas fa-exclamation-circle text-4xl text-red-500 mb-4"></i>
      <p class="text-red-600 dark:text-red-400">{{ error }}</p>
    </div>

    <!-- Challenge Content -->
    <div v-else class="w-full max-w-2xl">
      <!-- Timer -->
      <CountdownTimer @refresh="refreshChallenge" />

      <!-- Game Board -->
      <GameBoard
        :word="word"
        :guesses="guesses"
        :max-attempts="6"
        :interactive="!isCompleted"
        :is-daily-challenge="true"
        :current-score="currentScore"
        :guess-count="guessCount"
        :personal-best="personalBest"
        @guess="handleGuess"
      />

      <!-- Stats Button -->
      <div class="mt-8 text-center">
        <button
          @click="showStats = true"
          class="px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200"
        >
          <i class="fas fa-chart-bar mr-2 rtl:mr-0 rtl:ml-2"></i>
          {{ t('dailyChallengeStats') }}
        </button>
      </div>
    </div>

    <!-- Stats Modal -->
    <DailyStatsModal
      v-if="showStats"
      @close="showStats = false"
    />
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useTranslations } from '~/composables/useTranslations'
import { useDailyChallengeStore } from '~/stores/dailyChallenge'
import GameBoard from '~/components/GameBoard.vue'
import CountdownTimer from './CountdownTimer.vue'
import DailyStatsModal from './DailyStatsModal.vue'

const { t } = useTranslations()
const dailyStore = useDailyChallengeStore()

const loading = computed(() => dailyStore.loading)
const error = computed(() => dailyStore.error)
const word = computed(() => dailyStore.word)
const guesses = computed(() => dailyStore.guesses)
const isCompleted = computed(() => dailyStore.isCompleted)
const currentScore = computed(() => dailyStore.currentScore)
const personalBest = computed(() => dailyStore.personalBest)
const guessCount = computed(() => dailyStore.guessCount)
const showStats = ref(false)

// دریافت چالش روزانه
const refreshChallenge = async () => {
  await dailyStore.fetchTodaysChallenge()
}

// ارسال حدس
const handleGuess = async (guess) => {
  const result = await dailyStore.submitGuess(guess)
  if (result?.isGameOver) {
    showStats.value = true
  }
}

// شروع چالش
const startChallenge = async () => {
  await dailyStore.startParticipation()
}

// اجرای اولیه
onMounted(async () => {
  await refreshChallenge()
  await startChallenge()
})
</script>

<style scoped>
.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
</style> 