<template>
  <div class="theme-bg-primary rounded-lg p-6 shadow-lg">
    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center items-center h-40">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 theme-border-secondary"></div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="text-red-500 text-center py-4">
      {{ error }}
    </div>

    <!-- Data Display -->
    <div v-else class="space-y-6">
      <!-- Global Stats -->
      <div v-if="globalStats" class="space-y-4">
        <h3 class="text-xl font-bold theme-text-primary">آمار کلی بازی</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <StatBox title="تعداد کل بازی‌ها" :value="globalStats.totalGames" />
          <StatBox title="میانگین امتیاز" :value="globalStats.averageScore" />
          <StatBox title="بهترین امتیاز" :value="globalStats.highestScore" />
          <StatBox title="تعداد کاربران" :value="globalStats.totalUsers" />
        </div>
      </div>

      <!-- User Stats -->
      <div v-if="userStats" class="space-y-4">
        <h3 class="text-xl font-bold theme-text-primary">آمار شما</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <StatBox title="بازی‌های شما" :value="userStats.totalGames" />
          <StatBox title="برد" :value="userStats.wins" />
          <StatBox title="استریک فعلی" :value="userStats.currentStreak" />
          <StatBox title="بهترین استریک" :value="userStats.bestStreak" />
        </div>
      </div>

      <!-- Word Stats -->
      <div v-if="wordStats.mostPlayed.length" class="space-y-4">
        <h3 class="text-xl font-bold theme-text-primary">آمار کلمات</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <!-- Most Played -->
          <div class="space-y-2">
            <h4 class="font-semibold theme-text-secondary">پرتکرارترین کلمات</h4>
            <ul class="space-y-1">
              <li v-for="word in wordStats.mostPlayed" :key="word.word" 
                  class="flex justify-between items-center theme-text-primary">
                <span>{{ word.word }}</span>
                <span class="text-sm theme-text-secondary">{{ word.count }} بار</span>
              </li>
            </ul>
          </div>

          <!-- Hardest -->
          <div class="space-y-2">
            <h4 class="font-semibold theme-text-secondary">سخت‌ترین کلمات</h4>
            <ul class="space-y-1">
              <li v-for="word in wordStats.hardest" :key="word.word"
                  class="flex justify-between items-center theme-text-primary">
                <span>{{ word.word }}</span>
                <span class="text-sm theme-text-secondary">{{ word.failRate }}%</span>
              </li>
            </ul>
          </div>

          <!-- Easiest -->
          <div class="space-y-2">
            <h4 class="font-semibold theme-text-secondary">ساده‌ترین کلمات</h4>
            <ul class="space-y-1">
              <li v-for="word in wordStats.easiest" :key="word.word"
                  class="flex justify-between items-center theme-text-primary">
                <span>{{ word.word }}</span>
                <span class="text-sm theme-text-secondary">{{ word.successRate }}%</span>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useStatisticsStore } from '~/stores/statistics'
import { useAuthStore } from '~/stores/auth'
import { onMounted, computed } from 'vue'

const statisticsStore = useStatisticsStore()
const authStore = useAuthStore()

// Computed properties
const loading = computed(() => statisticsStore.isLoading)
const error = computed(() => statisticsStore.error)
const globalStats = computed(() => statisticsStore.getGlobalStats)
const userStats = computed(() => statisticsStore.getUserStats)
const wordStats = computed(() => ({
  mostPlayed: statisticsStore.getMostPlayedWords,
  hardest: statisticsStore.getHardestWords,
  easiest: statisticsStore.getEasiestWords
}))

// Load data on mount
onMounted(async () => {
  const token = authStore.getToken
  
  await Promise.all([
    statisticsStore.fetchGlobalStats(),
    token && statisticsStore.fetchUserStats(authStore.getUsername, token),
    token && statisticsStore.fetchWordStats(token)
  ])
})
</script>

<script>
// For component options
export default {
  name: 'StatisticsCard'
}
</script> 