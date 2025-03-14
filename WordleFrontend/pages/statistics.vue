<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 dark:from-gray-900 dark:to-gray-800">
    <div class="max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8">
      <div class="bg-white/80 dark:bg-gray-800/80 backdrop-blur-sm shadow-xl rounded-xl p-8">
        <h1 class="text-3xl font-bold text-gray-900 dark:text-white mb-8 font-display">{{ t('statistics') }}</h1>
        
        <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <!-- آمار کلی -->
          <div class="bg-gray-50 dark:bg-gray-700 rounded-xl p-6">
            <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-6">{{ t('overallStats') }}</h2>
            <div class="grid grid-cols-2 gap-6">
              <div class="text-center">
                <p class="text-3xl font-bold text-indigo-600 dark:text-indigo-400">{{ gamesPlayed }}</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('gamesPlayed') }}</p>
              </div>
              <div class="text-center">
                <p class="text-3xl font-bold text-indigo-600 dark:text-indigo-400">{{ winRate }}%</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('winRate') }}</p>
              </div>
              <div class="text-center">
                <p class="text-3xl font-bold text-indigo-600 dark:text-indigo-400">{{ currentStreak }}</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('currentStreak') }}</p>
              </div>
              <div class="text-center">
                <p class="text-3xl font-bold text-indigo-600 dark:text-indigo-400">{{ bestStreak }}</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('bestStreak') }}</p>
              </div>
            </div>
          </div>

          <!-- نمودار توزیع حدس‌ها -->
          <div class="bg-gray-50 dark:bg-gray-700 rounded-xl p-6">
            <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-6">{{ t('guessDistribution') }}</h2>
            <div class="space-y-4">
              <div v-for="i in 6" :key="i" class="flex items-center">
                <span class="w-8 text-gray-600 dark:text-gray-300">{{ i }}</span>
                <div class="flex-1 bg-gray-200 dark:bg-gray-600 rounded-full h-6 overflow-hidden">
                  <div
                    class="bg-indigo-600 h-full rounded-full transition-all duration-500"
                    :style="{ width: `${(guessDistribution[i] || 0) / maxGuessCount * 100}%` }"
                  ></div>
                </div>
                <span class="w-12 text-right text-gray-600 dark:text-gray-300">{{ guessDistribution[i] || 0 }}</span>
              </div>
            </div>
          </div>

          <!-- آمار سطح سختی -->
          <div class="bg-gray-50 dark:bg-gray-700 rounded-xl p-6">
            <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-6">{{ t('difficultyStats') }}</h2>
            <div class="grid grid-cols-3 gap-6">
              <div class="text-center">
                <p class="text-3xl font-bold text-green-600">{{ easyWins }}</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('easy') }}</p>
              </div>
              <div class="text-center">
                <p class="text-3xl font-bold text-yellow-600">{{ mediumWins }}</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('medium') }}</p>
              </div>
              <div class="text-center">
                <p class="text-3xl font-bold text-red-600">{{ hardWins }}</p>
                <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('hard') }}</p>
              </div>
            </div>
          </div>

          <!-- امتیازات -->
          <div class="bg-gray-50 dark:bg-gray-700 rounded-xl p-6">
            <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-6">{{ t('scores') }}</h2>
            <div class="text-center">
              <p class="text-4xl font-bold text-yellow-500">{{ totalScore }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('totalScore') }}</p>
              <div class="mt-6 grid grid-cols-2 gap-4">
                <div>
                  <p class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ highestScore }}</p>
                  <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('highestScore') }}</p>
                </div>
                <div>
                  <p class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ averageScore }}</p>
                  <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">{{ t('averageScore') }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useTranslations } from '../composables/useTranslations'

const { t } = useTranslations()

// این داده‌ها باید از store یا API دریافت شوند
const gamesPlayed = ref(0)
const winRate = ref(0)
const currentStreak = ref(0)
const bestStreak = ref(0)
const guessDistribution = ref({})
const maxGuessCount = ref(0)
const easyWins = ref(0)
const mediumWins = ref(0)
const hardWins = ref(0)
const totalScore = ref(0)
const highestScore = ref(0)
const averageScore = ref(0)

// اینجا باید داده‌ها از API یا store دریافت شوند
onMounted(async () => {
  // TODO: Load statistics data
})
</script> 