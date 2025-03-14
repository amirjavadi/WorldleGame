<template>
  <div :dir="dir" class="min-h-screen bg-gradient-to-br from-gray-100 to-gray-200 dark:from-gray-900 dark:to-gray-800 p-4">
    <div class="max-w-4xl mx-auto">
      <!-- Leaderboard Card -->
      <div class="bg-white dark:bg-gray-800 rounded-2xl p-8 shadow-2xl">
        <div class="flex items-center justify-between mb-8">
          <h1 class="text-3xl font-bold text-gray-900 dark:text-white">{{ t('leaderboard') }}</h1>
          
          <!-- Filter Options -->
          <div class="flex items-center space-x-4">
            <select
              v-model="timeFilter"
              class="bg-gray-50 dark:bg-gray-700 border border-gray-300 dark:border-gray-600 text-gray-900 dark:text-white rounded-lg px-4 py-2 focus:ring-2 focus:ring-green-500"
            >
              <option value="daily">{{ t('daily') }}</option>
              <option value="weekly">{{ t('weekly') }}</option>
              <option value="monthly">{{ t('monthly') }}</option>
              <option value="allTime">{{ t('allTime') }}</option>
            </select>
          </div>
        </div>

        <!-- Leaderboard Table -->
        <div class="overflow-x-auto">
          <table class="w-full">
            <thead>
              <tr class="text-left border-b border-gray-200 dark:border-gray-700">
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('rank') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('player') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('score') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('winRate') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('bestStreak') }}</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(player, index) in leaderboardData"
                :key="index"
                class="border-b border-gray-100 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors duration-200"
                :class="{ 'bg-green-50 dark:bg-green-900/20': player.isCurrentUser }"
              >
                <td class="py-4">
                  <div class="flex items-center">
                    <span
                      class="w-8 h-8 flex items-center justify-center rounded-full"
                      :class="getRankClass(index + 1)"
                    >
                      {{ index + 1 }}
                    </span>
                  </div>
                </td>
                <td class="py-4">
                  <div class="flex items-center">
                    <span class="font-medium text-gray-900 dark:text-white">
                      {{ player.username }}
                    </span>
                    <span
                      v-if="player.isCurrentUser"
                      class="ml-2 px-2 py-1 text-xs bg-green-100 dark:bg-green-900 text-green-800 dark:text-green-200 rounded-full"
                    >
                      {{ t('you') }}
                    </span>
                  </div>
                </td>
                <td class="py-4 text-gray-900 dark:text-white">{{ player.score }}</td>
                <td class="py-4 text-gray-900 dark:text-white">{{ player.winRate }}%</td>
                <td class="py-4 text-gray-900 dark:text-white">{{ player.bestStreak }}</td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Pagination -->
        <div class="mt-6 flex items-center justify-between">
          <button
            :disabled="currentPage === 1"
            @click="currentPage--"
            class="px-4 py-2 text-sm bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-600 disabled:opacity-50"
          >
            {{ t('previous') }}
          </button>
          <span class="text-gray-600 dark:text-gray-400">
            {{ t('page') }} {{ currentPage }} {{ t('of') }} {{ totalPages }}
          </span>
          <button
            :disabled="currentPage === totalPages"
            @click="currentPage++"
            class="px-4 py-2 text-sm bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-600 disabled:opacity-50"
          >
            {{ t('next') }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  middleware: ['auth']
})

const { t, dir } = useTranslations()
const timeFilter = ref('daily')
const currentPage = ref(1)
const totalPages = ref(1)

// Mock data - replace with actual API call
const leaderboardData = ref([
  {
    username: 'Player1',
    score: 1000,
    winRate: 85,
    bestStreak: 12,
    isCurrentUser: true
  },
  {
    username: 'Player2',
    score: 950,
    winRate: 80,
    bestStreak: 10,
    isCurrentUser: false
  },
  {
    username: 'Player3',
    score: 900,
    winRate: 75,
    bestStreak: 8,
    isCurrentUser: false
  }
])

const getRankClass = (rank) => {
  switch (rank) {
    case 1:
      return 'bg-yellow-100 dark:bg-yellow-900 text-yellow-800 dark:text-yellow-200'
    case 2:
      return 'bg-gray-100 dark:bg-gray-700 text-gray-800 dark:text-gray-200'
    case 3:
      return 'bg-orange-100 dark:bg-orange-900 text-orange-800 dark:text-orange-200'
    default:
      return 'bg-gray-50 dark:bg-gray-800 text-gray-600 dark:text-gray-400'
  }
}

// Watch for filter changes
watch(timeFilter, () => {
  // TODO: Fetch new data based on filter
  currentPage.value = 1
})

// Watch for page changes
watch(currentPage, () => {
  // TODO: Fetch new data based on page
})
</script> 