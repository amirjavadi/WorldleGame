import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    token: null,
    isAuthenticated: false
  }),

  getters: {
    currentUser: (state) => state.user,
    authToken: (state) => state.token,
    isLoggedIn: (state) => state.isAuthenticated
  },

  actions: {
    setUser(user) {
      this.user = user
      this.isAuthenticated = !!user
    },

    setToken(token) {
      this.token = token
    },

    async login(username, password) {
      try {
        const response = await $fetch(`${useRuntimeConfig().public.apiBase}/auth/login`, {
          method: 'POST',
          body: { username, password }
        })
        
        this.setToken(response.token)
        this.setUser(response.user)
        return response
      } catch (error) {
        throw error
      }
    },

    async register(email, username, password) {
      try {
        const response = await $fetch(`${useRuntimeConfig().public.apiBase}/auth/register`, {
          method: 'POST',
          body: { email, username, password }
        })
        
        this.setToken(response.token)
        this.setUser(response.user)
        return response
      } catch (error) {
        throw error
      }
    },

    logout() {
      this.setToken(null)
      this.setUser(null)
    }
  }
}) 