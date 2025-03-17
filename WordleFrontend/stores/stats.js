import { defineStore } from 'pinia'
import { useApi } from '~/composables/useApi'

export const useStatsStore = defineStore('stats', {
  state: () => ({
    loading: false,
    error: null,
    stats: {
      totalGames: 0,
      wins: 0,
      currentStreak: 0,
      maxStreak: 0,
      guessDistribution: {
        1: 0,
        2: 0,
        3: 0,
        4: 0,
        5: 0,
        6: 0
      },
      averageGuesses: 0,
      lastPlayed: null,
      lastWon: null
    }
  }),

  getters: {
    winRate: (state) => {
      if (state.stats.totalGames === 0) return 0
      return Math.round((state.stats.wins / state.stats.totalGames) * 100)
    },
    maxGuesses: (state) => {
      return Math.max(...Object.values(state.stats.guessDistribution))
    }
  },

  actions: {
    async fetchStats() {
      const api = useApi()
      this.loading = true
      this.error = null

      try {
        const response = await api.get('/stats')
        this.stats = response.data
      } catch (error) {
        this.error = error.message
        console.error('Error fetching stats:', error)
      } finally {
        this.loading = false
      }
    },

    async updateStats(gameResult) {
      const api = useApi()
      this.loading = true
      this.error = null

      try {
        const response = await api.post('/stats/update', gameResult)
        this.stats = response.data
      } catch (error) {
        this.error = error.message
        console.error('Error updating stats:', error)
      } finally {
        this.loading = false
      }
    }
  }
}) 