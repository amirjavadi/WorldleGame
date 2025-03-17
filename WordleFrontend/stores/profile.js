import { defineStore } from 'pinia'
import { useApi } from '~/composables/useApi'

export const useProfileStore = defineStore('profile', {
  state: () => ({
    loading: false,
    error: null,
    profile: {
      username: '',
      email: '',
      createdAt: null,
      stats: {
        totalGames: 0,
        wins: 0,
        currentStreak: 0,
        maxStreak: 0,
        averageGuesses: 0
      },
      recentGames: []
    }
  }),

  getters: {
    winRate: (state) => {
      if (state.profile.stats.totalGames === 0) return 0
      return Math.round((state.profile.stats.wins / state.profile.stats.totalGames) * 100)
    }
  },

  actions: {
    async fetchProfile() {
      const api = useApi()
      this.loading = true
      this.error = null

      try {
        const response = await api.get('/profile')
        this.profile = response.data
      } catch (error) {
        this.error = error.message
        console.error('Error fetching profile:', error)
      } finally {
        this.loading = false
      }
    },

    async updateProfile(profileData) {
      const api = useApi()
      this.loading = true
      this.error = null

      try {
        const response = await api.put('/profile', profileData)
        this.profile = response.data
      } catch (error) {
        this.error = error.message
        console.error('Error updating profile:', error)
      } finally {
        this.loading = false
      }
    },

    async fetchRecentGames() {
      const api = useApi()
      this.loading = true
      this.error = null

      try {
        const response = await api.get('/profile/games')
        this.profile.recentGames = response.data
      } catch (error) {
        this.error = error.message
        console.error('Error fetching recent games:', error)
      } finally {
        this.loading = false
      }
    }
  }
}) 