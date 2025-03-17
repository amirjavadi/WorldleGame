<template>
  <div class="container mx-auto px-4 py-8">
    <h2 class="text-2xl font-bold mb-6">{{ t('leaderboard') }}</h2>
    <div class="bg-secondary rounded-lg shadow-lg p-6">
      <div class="overflow-x-auto">
        <table class="min-w-full">
          <thead>
            <tr>
              <th class="px-6 py-3 text-left">{{ t('rank') }}</th>
              <th class="px-6 py-3 text-left">{{ t('username') }}</th>
              <th class="px-6 py-3 text-left">{{ t('score') }}</th>
              <th class="px-6 py-3 text-left">{{ t('wins') }}</th>
              <th class="px-6 py-3 text-left">{{ t('bestStreak') }}</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(player, index) in leaderboardData" :key="player.id" class="hover:bg-primary/10">
              <td class="px-6 py-4">{{ index + 1 }}</td>
              <td class="px-6 py-4">{{ player.username }}</td>
              <td class="px-6 py-4">{{ player.score }}</td>
              <td class="px-6 py-4">{{ player.wins }}</td>
              <td class="px-6 py-4">{{ player.bestStreak }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useApi } from '~/composables/useApi'
import { useTranslations } from '~/composables/useTranslations'

const { t } = useTranslations()
const api = useApi()
const leaderboardData = ref([])
const loading = ref(false)
const error = ref(null)

const fetchLeaderboard = async () => {
  loading.value = true
  error.value = null

  try {
    const response = await api.get('/leaderboard')
    leaderboardData.value = response.data
  } catch (e) {
    error.value = e.message
    console.error('Error fetching leaderboard:', e)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchLeaderboard()
})
</script> 