import { defineStore } from 'pinia'
import { api } from '~/services/api'
import { useAuthStore } from './auth'

export const useLeaderboardStore = defineStore('leaderboard', {
  state: () => ({
    dailyLeaderboard: [],
    weeklyLeaderboard: [],
    monthlyLeaderboard: [],
    allTimeLeaderboard: [],
    userStats: null,
    myStats: null,
    loading: false,
    error: null,
    lastUpdate: null,
    limit: 10
  }),

  actions: {
    async fetchDailyLeaderboard() {
      const auth = useAuthStore()
      try {
        this.loading = true
        this.error = null
        this.dailyLeaderboard = await api.leaderboard.getDaily(this.limit, auth.token)
        this.lastUpdate = new Date()
      } catch (error) {
        this.error = error.message
        this.dailyLeaderboard = []
      } finally {
        this.loading = false
      }
    },

    async fetchWeeklyLeaderboard() {
      const auth = useAuthStore()
      try {
        this.loading = true
        this.error = null
        this.weeklyLeaderboard = await api.leaderboard.getWeekly(this.limit, auth.token)
        this.lastUpdate = new Date()
      } catch (error) {
        this.error = error.message
        this.weeklyLeaderboard = []
      } finally {
        this.loading = false
      }
    },

    async fetchMonthlyLeaderboard() {
      const auth = useAuthStore()
      try {
        this.loading = true
        this.error = null
        this.monthlyLeaderboard = await api.leaderboard.getMonthly(this.limit, auth.token)
        this.lastUpdate = new Date()
      } catch (error) {
        this.error = error.message
        this.monthlyLeaderboard = []
      } finally {
        this.loading = false
      }
    },

    async fetchAllTimeLeaderboard() {
      const auth = useAuthStore()
      try {
        this.loading = true
        this.error = null
        this.allTimeLeaderboard = await api.leaderboard.getAllTime(this.limit, auth.token)
        this.lastUpdate = new Date()
      } catch (error) {
        this.error = error.message
        this.allTimeLeaderboard = []
      } finally {
        this.loading = false
      }
    },

    async fetchUserStats(userId, period = 'all-time') {
      const auth = useAuthStore()
      try {
        this.loading = true
        this.error = null
        this.userStats = await api.leaderboard.getUserStats(userId, period, auth.token)
      } catch (error) {
        this.error = error.message
        this.userStats = null
      } finally {
        this.loading = false
      }
    },

    async fetchMyStats(period = 'all-time') {
      const auth = useAuthStore()
      try {
        this.loading = true
        this.error = null
        this.myStats = await api.leaderboard.getMyStats(period, auth.token)
      } catch (error) {
        this.error = error.message
        this.myStats = null
      } finally {
        this.loading = false
      }
    },

    setLimit(newLimit) {
      this.limit = newLimit
    },

    resetError() {
      this.error = null
    }
  },

  getters: {
    needsUpdate: (state) => {
      if (!state.lastUpdate) return true
      const now = new Date()
      const fiveMinutes = 5 * 60 * 1000
      return now - state.lastUpdate > fiveMinutes
    }
  }
}) 