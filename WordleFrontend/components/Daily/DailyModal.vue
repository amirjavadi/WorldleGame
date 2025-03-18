<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[60]">
    <div class="bg-white dark:bg-gray-800 rounded-xl p-8 max-w-2xl w-full mx-4 transform transition-all duration-300">
      <!-- Header -->
      <div class="flex justify-between items-center mb-6">
        <div class="flex items-center space-x-4 rtl:space-x-reverse">
          <div class="w-12 h-12 bg-gradient-to-br from-yellow-400 to-amber-500 rounded-full flex items-center justify-center">
            <i class="fas fa-trophy text-2xl text-white"></i>
          </div>
          <div>
            <h3 class="text-2xl font-bold text-gray-900 dark:text-white font-display">{{ t('dailyChallenge') }}</h3>
            <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('dailyChallengeDescription') }}</p>
          </div>
        </div>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
        <i class="fas fa-times text-xl"></i>
      </button>
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

      <!-- Not Authenticated State -->
      <div v-else-if="!isAuthenticated" class="text-center py-8">
        <i class="fas fa-user-lock text-4xl text-gray-400 mb-4"></i>
        <p class="text-gray-600 dark:text-gray-300">{{ t('loginRequiredForDaily') }}</p>
          </div>

      <!-- Game Section -->
      <div v-else class="space-y-6">
        <!-- Timer -->
        <div class="bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/20 dark:to-purple-900/20 p-4 rounded-lg">
          <div class="text-center">
            <p class="text-sm text-indigo-600 dark:text-indigo-400 mb-2">{{ t('nextChallengeIn') }}</p>
          <div class="flex justify-center space-x-4 rtl:space-x-reverse">
              <div class="text-center">
                <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ hours }}</div>
                <div class="text-sm text-indigo-500 dark:text-indigo-500">{{ t('hours') }}</div>
              </div>
              <div class="text-center">
                <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ minutes }}</div>
                <div class="text-sm text-indigo-500 dark:text-indigo-500">{{ t('minutes') }}</div>
          </div>
              <div class="text-center">
                <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ seconds }}</div>
                <div class="text-sm text-indigo-500 dark:text-indigo-500">{{ t('seconds') }}</div>
        </div>
            </div>
          </div>
        </div>

        <!-- Game Board -->
        <GameBoard
          :word="challenge?.word?.text"
          :guesses="guesses"
          :max-attempts="6"
          :show-score="true"
          :score="currentScore"
          :personal-best="personalBest"
          :is-interactive="isInProgress"
          @make-guess="handleGuess"
        />

        <!-- Leaderboard -->
        <div v-if="leaderboard.length > 0" class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-4">
          <h4 class="text-lg font-bold text-gray-900 dark:text-white mb-4">{{ t('leaderboard') }}</h4>
          <div class="space-y-3">
            <div
              v-for="(player, index) in leaderboard.slice(0, 3)"
              :key="player.username"
              class="flex items-center justify-between p-3 rounded-lg"
              :class="[
                index === 0 ? 'bg-gradient-to-r from-yellow-50 to-amber-50 dark:from-yellow-900/20 dark:to-amber-900/20' :
                index === 1 ? 'bg-gradient-to-r from-gray-50 to-slate-50 dark:from-gray-800/20 dark:to-slate-800/20' :
                index === 2 ? 'bg-gradient-to-r from-orange-50 to-rose-50 dark:from-orange-900/20 dark:to-rose-900/20' :
                'bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/10 dark:to-purple-900/10'
              ]"
            >
              <div class="flex items-center space-x-3 rtl:space-x-reverse">
                <div
                  class="w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold"
                  :class="[
                    index === 0 ? 'bg-yellow-400 text-yellow-900' :
                    index === 1 ? 'bg-gray-400 text-gray-900' :
                    index === 2 ? 'bg-orange-400 text-orange-900' :
                    'bg-indigo-400 text-indigo-900'
                  ]"
                >
                  {{ index + 1 }}
                </div>
                <span class="font-medium text-gray-900 dark:text-white">{{ player.username }}</span>
              </div>
              <div class="text-right">
                <div class="text-lg font-bold text-gray-900 dark:text-white">{{ player.score }}</div>
                <div class="text-sm text-gray-500 dark:text-gray-400">{{ player.attempts }}/6</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useTranslations } from '~/composables/useTranslations'
import { useDailyChallengeStore } from '~/stores/dailyChallenge'
import { useAuthStore } from '~/stores/auth'
import { api } from '~/services/api'
import GameBoard from '~/components/GameBoard.vue'

const { t } = useTranslations()
const dailyStore = useDailyChallengeStore()
const authStore = useAuthStore()

const isAuthenticated = computed(() => authStore.isLoggedIn)
const loading = computed(() => dailyStore.loading)
const error = computed(() => dailyStore.error)
const challenge = computed(() => dailyStore.challenge)
const participation = computed(() => dailyStore.participation)
const leaderboard = computed(() => dailyStore.leaderboard)
const guesses = computed(() => dailyStore.guesses)
const currentScore = computed(() => dailyStore.currentScore)
const isInProgress = computed(() => dailyStore.isInProgress)

// محاسبه زمان باقی‌مانده
const timeUntilNext = computed(() => dailyStore.timeUntilNextChallenge)
const hours = computed(() => Math.floor(timeUntilNext.value / (1000 * 60 * 60)))
const minutes = computed(() => Math.floor((timeUntilNext.value % (1000 * 60 * 60)) / (1000 * 60)))
const seconds = computed(() => Math.floor((timeUntilNext.value % (1000 * 60)) / 1000))

// دریافت رکورد شخصی از localStorage
const personalBest = computed(() => {
  if (process.client) {
    const best = localStorage.getItem('dailyChallengeBest')
    return best ? parseInt(best) : null
  }
  return null
})

// ذخیره رکورد شخصی در localStorage
const updatePersonalBest = (score) => {
  if (process.client) {
    const currentBest = localStorage.getItem('dailyChallengeBest')
    if (!currentBest || score > parseInt(currentBest)) {
      localStorage.setItem('dailyChallengeBest', score.toString())
    }
  }
}

// شروع تایمر
let timerInterval
onMounted(async () => {
  if (isAuthenticated.value) {
    await Promise.all([
      dailyStore.fetchTodaysChallenge(),
      dailyStore.fetchUserParticipation(),
      dailyStore.fetchLeaderboard()
    ])

    if (!participation.value) {
      await dailyStore.startParticipation()
    }

    timerInterval = setInterval(() => {
      if (timeUntilNext.value <= 0) {
        clearInterval(timerInterval)
        // رفرش صفحه برای دریافت چالش جدید
        window.location.reload()
      }
    }, 1000)
  }
})

onUnmounted(() => {
  if (timerInterval) {
    clearInterval(timerInterval)
  }
})

// مدیریت حدس
const handleGuess = async (guess) => {
  try {
    await dailyStore.submitGuess(guess)
    if (dailyStore.hasWon) {
      updatePersonalBest(dailyStore.currentScore)
    }
  } catch (err) {
    console.error('Error submitting guess:', err)
  }
}

defineEmits(['close', 'login'])
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