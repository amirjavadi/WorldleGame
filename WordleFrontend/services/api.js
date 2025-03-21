import { useRuntimeConfig } from '#app'
import axios from 'axios'
import { useAuthStore } from '~/stores/auth'

class ApiService {
  constructor() {
    this.baseURL = ''
  }

  initialize() {
    const config = useRuntimeConfig()
    this.baseURL = config.public.apiBase
  }

  async verifyTokenBeforeRequest() {
    if (process.client) {
      const authStore = useAuthStore()
      if (!authStore.isTokenChecked) {
        await authStore.init()
      }
      if (!authStore.isLoggedIn || authStore.isGuest) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      if (!authStore.token) {
        throw new Error('توکن شما منقضی شده است. لطفاً دوباره وارد شوید')
      }
      return authStore.token
    }
    return false
  }

  async request(endpoint, options = {}) {
    if (!this.baseURL) {
      this.initialize()
    }

    const url = `${this.baseURL}/api${endpoint}`
    const headers = {
      'Content-Type': 'application/json'
    }

    if (options.token) {
      headers.Authorization = `Bearer ${options.token}`
    }

    try {
      const config = {
        method: options.method || 'GET',
        headers,
        ...options
      }

      if (options.body) {
        config.body = options.body
      }

      delete config.token // حذف توکن از آپشن‌ها چون در هدر اضافه شده

      const response = await fetch(url, config)

      if (!response.ok) {
        if (response.status === 401) {
          throw new Error('لطفاً دوباره وارد شوید')
        }
        const error = await response.json().catch(() => ({ message: 'خطای سرور' }))
        throw new Error(error.message || 'خطای سرور')
      }

      const data = await response.json()
      return data
    } catch (error) {
      console.error(`API Error (${endpoint}):`, error)
      throw error
    }
  }

  // Auth APIs
  auth = {
    login: (username, password) => 
      this.request('/auth/login', {
        method: 'POST',
        body: JSON.stringify({ username, password })
      }),

    register: (username, email, password) =>
      this.request('/auth/register', {
        method: 'POST',
        body: JSON.stringify({ username, email, password })
      }),

    verify: (token) =>
      this.request('/auth/verify', {
        method: 'GET',
        token
      })
  }

  // Game APIs
  game = {
    getWord: () => 
      this.request('/game/word'),

    submitGuess: (guess, token) =>
      this.request('/game/guess', {
        method: 'POST',
        body: JSON.stringify({ guess }),
        token
      }),

    getStats: (token) =>
      this.request('/game/stats', {
        token
      })
  }

  // Leaderboard APIs
  leaderboard = {
    getDaily: (limit = 10, token) => 
      this.request(`/leaderboard/daily?limit=${limit}`, {
        token
      }),
      
    getWeekly: (limit = 10, token) =>
      this.request(`/leaderboard/weekly?limit=${limit}`, {
        token
      }),
      
    getMonthly: (limit = 10, token) =>
      this.request(`/leaderboard/monthly?limit=${limit}`, {
        token
      }),
      
    getAllTime: (limit = 10, token) =>
      this.request(`/leaderboard/all-time?limit=${limit}`, {
        token
      }),
      
    getUserStats: (userId, period = 'all-time', token) =>
      this.request(`/leaderboard/user/${userId}?period=${period}`, {
        token
      }),
      
    getMyStats: (period = 'all-time', token) =>
      this.request(`/leaderboard/me?period=${period}`, {
        token
      })
  }

  // Admin APIs
  admin = {
    addWord: (word, token) =>
      this.request('/admin/words', {
        method: 'POST',
        body: JSON.stringify({ word }),
        token
      }),

    getWords: (token) =>
      this.request('/admin/words', {
        token
      })
  }

  // Statistics APIs
  statistics = {
    // دریافت آمار کلی بازی
    getGlobal: () => 
      this.request('/statistics/global'),

    // دریافت آمار کاربر
    getUser: (userId, token) =>
      this.request(`/statistics/user/${userId}`, {
        token
      }),

    // دریافت آمار پروفایل کاربر
    getProfile: (token) =>
      this.request('/user-profile/stats', {
        token
      }),

    // دریافت آمار من در جدول امتیازات
    getMyStats: (token) =>
      this.request('/leaderboard/my-stats', {
        token
      }),

    // دریافت آمار روزانه
    getDaily: (startDate, endDate, token) =>
      this.request('/statistics/daily', {
        method: 'GET',
        params: { startDate, endDate },
        token
      }),

    // دریافت کلمات پرتکرار
    getMostPlayed: (limit = 10, token) =>
      this.request('/statistics/words/most-played', {
        params: { limit },
        token
      }),

    // دریافت سخت‌ترین کلمات
    getHardest: (limit = 10, token) =>
      this.request('/statistics/words/hardest', {
        params: { limit },
        token
      }),

    // دریافت ساده‌ترین کلمات
    getEasiest: (limit = 10, token) =>
      this.request('/statistics/words/easiest', {
        params: { limit },
        token
      })
  }

  // Daily Challenge APIs
  daily = {
    // دریافت چالش امروز
    getTodaysChallenge: async () => {
      return await this.request('/dailychallenge/today', {
        method: 'GET'
      })
    },

    // شروع مشارکت در چالش
    participate: async () => {
      const token = await this.verifyTokenBeforeRequest()
      if (!token) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      return await this.request('/dailychallenge/participate', {
        method: 'POST',
        token
      })
    },

    // ارسال حدس
    submitGuess: async (participationId, guess) => {
      const token = await this.verifyTokenBeforeRequest()
      if (!token) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      return await this.request(`/dailychallenge/${participationId}/guess`, {
        method: 'POST',
        body: JSON.stringify({ guess }),
        token
      })
    },

    // دریافت جدول امتیازات
    getLeaderboard: async (date = null, limit = 10) => {
      const token = await this.verifyTokenBeforeRequest()
      if (!token) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      const params = new URLSearchParams()
      if (date) params.append('date', date)
      if (limit) params.append('limit', limit)
      return await this.request(`/dailychallenge/leaderboard?${params}`, {
        method: 'GET',
        token
      })
    },

    // دریافت وضعیت مشارکت کاربر
    getParticipation: async (date = null) => {
      try {
        const token = await this.verifyTokenBeforeRequest()
        const params = new URLSearchParams()
        if (date) params.append('date', date)
        const response = await this.request(`/dailychallenge/participation?${params}`, {
          method: 'GET',
          token
        })
        return response
      } catch (error) {
        if (error.message.includes('توکن')) {
          // اگر توکن منقضی شده، کاربر را به صفحه لاگین هدایت می‌کنیم
          const authStore = useAuthStore()
          await authStore.logout()
          navigateTo('/login')
        }
        throw error
      }
    },

    // دریافت چالش‌های گذشته
    getPastChallenges: async (days = 7) => {
      const token = await this.verifyTokenBeforeRequest()
      if (!token) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      return await this.request(`/dailychallenge/past-challenges?days=${days}`, {
        method: 'GET',
        token
      })
    },

    // دریافت آمار چالش روزانه
    getDailyStats: async (date = null) => {
      const token = await this.verifyTokenBeforeRequest()
      if (!token) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      const params = new URLSearchParams()
      if (date) params.append('date', date)
      return await this.request(`/dailychallenge/stats?${params}`, {
        method: 'GET',
        token
      })
    },

    // دریافت رتبه کاربر در چالش روزانه
    getUserRank: async (date = null) => {
      const token = await this.verifyTokenBeforeRequest()
      if (!token) {
        throw new Error('لطفاً ابتدا وارد شوید')
      }
      const params = new URLSearchParams()
      if (date) params.append('date', date)
      return await this.request(`/dailychallenge/rank?${params}`, {
        method: 'GET',
        token
      })
    }
  }
}

// Create a singleton instance
const api = new ApiService()

// Add interceptor to handle 401 errors globally
axios.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      // Convert all 401 errors to a consistent message
      throw new Error('لطفاً دوباره وارد شوید')
    }
    throw error
  }
)

// Export the singleton as a named export
export { api } 