<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[60]">
    <div class="bg-white dark:bg-gray-800 rounded-xl p-8 max-w-2xl w-full mx-4 transform transition-all duration-300">
      <!-- Header -->
      <div class="flex justify-between items-center mb-6">
        <div class="flex items-center space-x-4 rtl:space-x-reverse">
          <div class="w-12 h-12 bg-gradient-to-br from-indigo-400 to-purple-500 rounded-full flex items-center justify-center">
            <i class="fas fa-chart-bar text-2xl text-white"></i>
          </div>
          <div>
            <h3 class="text-2xl font-bold text-gray-900 dark:text-white font-display">{{ t('dailyChallengeStats') }}</h3>
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

      <!-- Stats Section -->
      <div v-else class="space-y-6">
        <!-- Personal Stats -->
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <div class="bg-gradient-to-br from-indigo-50 to-purple-50 dark:from-indigo-900/20 dark:to-purple-900/20 p-4 rounded-lg text-center">
            <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ currentScore }}</div>
            <div class="text-sm text-indigo-500 dark:text-indigo-500">{{ t('score') }}</div>
          </div>
          <div class="bg-gradient-to-br from-yellow-50 to-amber-50 dark:from-yellow-900/20 dark:to-amber-900/20 p-4 rounded-lg text-center">
            <div class="text-2xl font-bold text-yellow-600 dark:text-yellow-400">{{ personalBest }}</div>
            <div class="text-sm text-yellow-500 dark:text-yellow-500">{{ t('dailyChallengeBest') }}</div>
          </div>
          <div class="bg-gradient-to-br from-green-50 to-emerald-50 dark:from-green-900/20 dark:to-emerald-900/20 p-4 rounded-lg text-center">
            <div class="text-2xl font-bold text-green-600 dark:text-green-400">{{ streak }}</div>
            <div class="text-sm text-green-500 dark:text-green-500">{{ t('dailyChallengeStreak') }}</div>
          </div>
          <div class="bg-gradient-to-br from-blue-50 to-cyan-50 dark:from-blue-900/20 dark:to-cyan-900/20 p-4 rounded-lg text-center">
            <div class="text-2xl font-bold text-blue-600 dark:text-blue-400">{{ winRate }}%</div>
            <div class="text-sm text-blue-500 dark:text-blue-500">{{ t('dailyChallengeWinRate') }}</div>
          </div>
        </div>

        <!-- Share Result -->
        <div v-if="hasWon" class="bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/20 dark:to-purple-900/20 p-4 rounded-lg">
          <div class="flex items-center justify-between">
            <div>
              <h4 class="text-lg font-bold text-gray-900 dark:text-white">{{ t('dailyChallengeShare') }}</h4>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('dailyChallengeShareText', { attempts: guessCount }) }}</p>
            </div>
            <div class="flex space-x-2 rtl:space-x-reverse">
              <button
                @click="copyResult"
                class="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200"
              >
                <i class="fas fa-copy mr-2 rtl:mr-0 rtl:ml-2"></i>
                {{ t('dailyChallengeCopyButton') }}
              </button>
              <button
                @click="shareResult"
                class="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transform hover:scale-105 transition-all duration-200"
              >
                <i class="fas fa-share-alt mr-2 rtl:mr-0 rtl:ml-2"></i>
                {{ t('dailyChallengeShareButton') }}
              </button>
            </div>
          </div>
        </div>

        <!-- Leaderboard -->
        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-4">
          <h4 class="text-lg font-bold text-gray-900 dark:text-white mb-4">{{ t('dailyChallengeLeaderboardTitle') }}</h4>
          <div class="space-y-3">
            <div
              v-for="(player, index) in leaderboard.slice(0, 10)"
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
import { ref, computed, onMounted } from 'vue'
import { useTranslations } from '~/composables/useTranslations'
import { useDailyChallengeStore } from '~/stores/dailyChallenge'

const { t } = useTranslations()
const dailyStore = useDailyChallengeStore()

const loading = computed(() => dailyStore.loading)
const error = computed(() => dailyStore.error)
const currentScore = computed(() => dailyStore.currentScore)
const personalBest = computed(() => dailyStore.personalBest)
const streak = computed(() => dailyStore.streak)
const winRate = computed(() => dailyStore.winRate)
const guessCount = computed(() => dailyStore.guessCount)
const hasWon = computed(() => dailyStore.hasWon)
const leaderboard = computed(() => dailyStore.leaderboard)
const stats = computed(() => dailyStore.stats)

// دریافت آمار و رتبه‌بندی
const fetchData = async () => {
  await Promise.all([
    dailyStore.fetchLeaderboard(),
    dailyStore.fetchDailyStats()
  ])
}

// اجرای اولیه
onMounted(async () => {
  await fetchData()
})

// کپی نتیجه
const copyResult = async () => {
  try {
    const text = t('dailyChallengeShareText', { 
      attempts: guessCount.value,
      score: currentScore.value,
      totalPlayers: stats.value?.totalPlayers || 0,
      averageScore: stats.value?.averageScore || 0
    })
    await navigator.clipboard.writeText(text)
    // نمایش پیام موفقیت
  } catch (err) {
    console.error('Error copying result:', err)
    // نمایش پیام خطا
  }
}

// اشتراک نتیجه
const shareResult = async () => {
  try {
    const text = t('dailyChallengeShareText', { 
      attempts: guessCount.value,
      score: currentScore.value,
      totalPlayers: stats.value?.totalPlayers || 0,
      averageScore: stats.value?.averageScore || 0
    })
    if (navigator.share) {
      await navigator.share({
        title: t('dailyChallenge'),
        text,
        url: window.location.href
      })
    } else {
      // اگر Web Share API پشتیبانی نمی‌شود، از کپی استفاده کنید
      await copyResult()
    }
  } catch (err) {
    console.error('Error sharing result:', err)
    // نمایش پیام خطا
  }
}

defineEmits(['close'])
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