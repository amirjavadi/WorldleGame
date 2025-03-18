import { defineStore } from 'pinia'
import { api } from '~/services/api'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    token: null,
    username: null,
    isGuest: false,
    loading: false,
    error: null,
    showGuestModal: false,
    tokenChecked: false
  }),

  getters: {
    isLoggedIn: (state) => state.isAuthenticated && !state.isGuest && state.token,
    getToken: (state) => state.token,
    getUsername: (state) => state.username,
    isGuestUser: (state) => state.isGuest,
    shouldShowGuestModal: (state) => state.showGuestModal,
    isTokenChecked: (state) => state.tokenChecked
  },

  actions: {
    async init() {
      if (this.token && !this.tokenChecked) {
        const isValid = await this.checkAuth();
        this.tokenChecked = true;
        if (!isValid) {
          this.resetState();
          if (process.client) {
            navigateTo('/login');
          }
        }
        return isValid;
      }
      this.tokenChecked = true;
      return false;
    },

    async checkAuth() {
      if (!this.token) {
        this.resetState();
        return false;
      }
      
      try {
        const data = await api.auth.verify(this.token);
        
        if (data.isValid) {
          this.isAuthenticated = true;
          if (data.username) {
            this.username = data.username;
          }
          return true;
        }

        this.resetState();
        return false;
      } catch (error) {
        console.error('Auth check failed:', error);
        this.resetState();
        return false;
      }
    },

    async login(credentials) {
      this.loading = true;
      this.error = null;
      
      try {
        const data = await api.auth.login(credentials.username, credentials.password);

        if (data.token) {
          this.token = data.token;
          this.username = data.username || credentials.username;
          this.isAuthenticated = true;
          this.isGuest = false;
          this.tokenChecked = true;
          return true;
        }
        
        throw new Error(data.message || 'Login failed');
      } catch (error) {
        console.error('Login error:', error);
        this.error = error.message || 'خطا در ورود به سیستم';
        return false;
      } finally {
        this.loading = false;
      }
    },

    loginAsGuest() {
      this.resetState();
      this.isAuthenticated = true;
      this.isGuest = true;
      this.username = 'مهمان';
      this.showGuestModal = true;
      this.tokenChecked = true;
    },

    hideGuestModal() {
      this.showGuestModal = false;
    },

    logout() {
      this.resetState();
      if (process.client) {
        navigateTo('/login');
      }
    },

    resetState() {
      this.isAuthenticated = false;
      this.token = null;
      this.username = null;
      this.isGuest = false;
      this.error = null;
      this.showGuestModal = false;
      this.tokenChecked = false;
      
      // Clear persisted state
      if (process.client) {
        localStorage.removeItem('auth-store');
      }
    }
  },

  persist: {
    key: 'auth-store',
    paths: ['isAuthenticated', 'token', 'username', 'isGuest']
  }
}) 