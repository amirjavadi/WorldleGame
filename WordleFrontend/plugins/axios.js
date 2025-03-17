import axios from 'axios'
import { useAuthStore } from '~/stores/auth'

export default defineNuxtPlugin(async () => {
  // تنظیم آدرس پایه API
  axios.defaults.baseURL = 'http://localhost:5275'

  // تنظیم headers پیش‌فرض
  axios.defaults.headers.common['Content-Type'] = 'application/json'
  
  // اضافه کردن interceptor برای تنظیم توکن
  axios.interceptors.request.use(async config => {
    if (process.client) {
      const authStore = useAuthStore()
      
      // Skip token validation for auth-related endpoints
      const isAuthEndpoint = config.url.includes('/auth/') || config.url.includes('/verify')
      
      if (!isAuthEndpoint && authStore.token) {
        // Validate token before making request
        const isValid = await authStore.checkAuth()
        if (!isValid) {
          authStore.resetState()
          if (process.client) {
            navigateTo('/login')
          }
          throw new Error('Invalid token')
        }
      }
      
      const token = authStore.getToken
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
    }
    return config
  }, error => {
    return Promise.reject(error)
  })

  // تنظیم interceptor برای مدیریت خطاها
  axios.interceptors.response.use(
    response => response,
    async error => {
      if (error.response?.status === 401 && process.client) {
        const authStore = useAuthStore()
        authStore.resetState()
        if (process.client) {
          navigateTo('/login')
        }
      }
      return Promise.reject(error)
    }
  )
}) 