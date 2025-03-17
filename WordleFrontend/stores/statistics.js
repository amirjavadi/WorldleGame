import { defineStore } from 'pinia'
import api from '~/services/api'
import { useAuthStore } from '~/stores/auth'

export const useStatisticsStore = defineStore('statistics', {
  state: () => ({
    loading: false,
    error: null,
    globalStats: null,
    userStats: null,
    profileStats: null,
    leaderboardStats: null,
    dailyStats: [],
    wordStats: {
      mostPlayed: [],
      hardest: [],
      easiest: []
    }
  }),

  getters: {
    hasError: (state) => !!state.error,
    isLoading: (state) => state.loading,
    getGlobalStats: (state) => state.globalStats,
    getUserStats: (state) => state.userStats,
    getProfileStats: (state) => state.profileStats,
    getLeaderboardStats: (state) => state.leaderboardStats,
    getDailyStats: (state) => state.dailyStats,
    getMostPlayedWords: (state) => state.wordStats.mostPlayed,
    getHardestWords: (state) => state.wordStats.hardest,
    getEasiestWords: (state) => state.wordStats.easiest
  },

  actions: {
    // دریافت آمار کلی
    async fetchGlobalStats() {
      const auth = useAuthStore();
      if (auth.isGuest) {
        this.globalStats = this.getEmptyStats();
        return true;
      }

      this.loading = true;
      this.error = null;
      try {
        const data = await api.statistics.getGlobal();
        this.globalStats = data;
        return true;
      } catch (error) {
        console.error('Error fetching global stats:', error);
        this.error = 'خطا در دریافت آمار کلی';
        return false;
      } finally {
        this.loading = false;
      }
    },

    // دریافت آمار کاربر
    async fetchUserStats(userId, token) {
      const auth = useAuthStore();
      if (auth.isGuest) {
        this.userStats = this.getEmptyStats();
        return true;
      }

      this.loading = true;
      this.error = null;
      try {
        const data = await api.statistics.getUser(userId, token);
        this.userStats = data;
        return true;
      } catch (error) {
        console.error('Error fetching user stats:', error);
        this.error = 'خطا در دریافت آمار کاربر';
        return false;
      } finally {
        this.loading = false;
      }
    },

    // دریافت آمار پروفایل
    async fetchProfileStats(token) {
      const auth = useAuthStore();
      if (auth.isGuest) {
        this.profileStats = this.getEmptyStats();
        return true;
      }

      this.loading = true;
      this.error = null;
      try {
        const data = await api.statistics.getProfile(token);
        this.profileStats = data;
        return true;
      } catch (error) {
        console.error('Error fetching profile stats:', error);
        this.error = 'خطا در دریافت آمار پروفایل';
        return false;
      } finally {
        this.loading = false;
      }
    },

    // دریافت آمار من در لیدربورد
    async fetchMyLeaderboardStats(token) {
      const auth = useAuthStore();
      if (auth.isGuest) {
        this.leaderboardStats = this.getEmptyStats();
        return true;
      }

      this.loading = true;
      this.error = null;
      try {
        const data = await api.statistics.getMyStats(token);
        this.leaderboardStats = data;
        return true;
      } catch (error) {
        console.error('Error fetching leaderboard stats:', error);
        this.error = 'خطا در دریافت آمار لیدربورد';
        return false;
      } finally {
        this.loading = false;
      }
    },

    // دریافت آمار روزانه
    async fetchDailyStats(startDate, endDate, token) {
      const auth = useAuthStore();
      if (auth.isGuest) {
        this.dailyStats = [];
        return true;
      }

      this.loading = true;
      this.error = null;
      try {
        const data = await api.statistics.getDaily(startDate, endDate, token);
        this.dailyStats = data;
        return true;
      } catch (error) {
        console.error('Error fetching daily stats:', error);
        this.error = 'خطا در دریافت آمار روزانه';
        return false;
      } finally {
        this.loading = false;
      }
    },

    // دریافت آمار کلمات
    async fetchWordStats(token) {
      const auth = useAuthStore();
      if (auth.isGuest) {
        this.wordStats = {
          mostPlayed: [],
          hardest: [],
          easiest: []
        };
        return true;
      }

      this.loading = true;
      this.error = null;
      try {
        const [mostPlayed, hardest, easiest] = await Promise.all([
          api.statistics.getMostPlayed(10, token),
          api.statistics.getHardest(10, token),
          api.statistics.getEasiest(10, token)
        ]);

        this.wordStats = {
          mostPlayed,
          hardest,
          easiest
        };
        return true;
      } catch (error) {
        console.error('Error fetching word stats:', error);
        this.error = 'خطا در دریافت آمار کلمات';
        return false;
      } finally {
        this.loading = false;
      }
    },

    // پاک کردن خطا
    clearError() {
      this.error = null;
    },

    // پاک کردن داده‌ها
    clearData() {
      this.globalStats = null;
      this.userStats = null;
      this.profileStats = null;
      this.leaderboardStats = null;
      this.dailyStats = [];
      this.wordStats = {
        mostPlayed: [],
        hardest: [],
        easiest: []
      };
    },

    // ایجاد آمار خالی برای کاربر مهمان
    getEmptyStats() {
      return {
        totalGames: 0,
        gamesWon: 0,
        gamesLost: 0,
        currentStreak: 0,
        bestStreak: 0,
        winRate: 0,
        averageAttempts: 0,
        lastPlayed: null
      };
    }
  },

  persist: {
    key: 'statistics-store',
    paths: ['globalStats', 'userStats', 'profileStats', 'leaderboardStats', 'dailyStats', 'wordStats']
  }
}) 