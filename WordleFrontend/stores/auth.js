import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import axios from 'axios'
import { useNotification } from '~/composables/useNotification'
import { useRouter } from 'vue-router'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(null)
  const loading = ref(false)
  const { addNotification } = useNotification()
  const router = useRouter()

  const isAuthenticated = computed(() => {
    console.log('Auth state:', { token: token.value, user: user.value })
    return !!token.value && !!user.value
  })
  const username = computed(() => user.value?.username)

  const initializeAuth = () => {
    console.log('Initializing auth...')
    if (process.client) {
      const savedToken = localStorage.getItem('auth-token')
      const savedUser = localStorage.getItem('auth-user')
      console.log('Saved data:', { savedToken, savedUser })
      
      if (savedToken && savedUser) {
        token.value = savedToken
        try {
          user.value = JSON.parse(savedUser)
        } catch {
          user.value = savedUser
        }
        console.log('Auth initialized with:', { token: token.value, user: user.value })
        axios.defaults.headers.common['Authorization'] = `Bearer ${savedToken}`
      }
    }
  }

  const register = async (userData) => {
    try {
      loading.value = true
      const response = await axios.post('/api/auth/register', userData)
      addNotification('ثبت‌نام با موفقیت انجام شد. لطفاً وارد شوید.', 'success')
      router.push('/login')
      return true
    } catch (error) {
      console.error('Register error:', error)
      addNotification(error.response?.data?.message || 'خطا در ثبت‌نام', 'error')
      return false
    } finally {
      loading.value = false
    }
  }

  const login = async (credentials) => {
    try {
      loading.value = true
      const response = await axios.post('/api/auth/login', credentials)
      console.log('Login response:', response.data)
      const { token: newToken, user: userData } = response.data
      
      token.value = newToken
      user.value = userData
      
      if (process.client) {
        localStorage.setItem('auth-token', newToken)
        localStorage.setItem('auth-user', JSON.stringify(userData))
      }
      
      axios.defaults.headers.common['Authorization'] = `Bearer ${newToken}`
      
      addNotification('خوش آمدید!', 'success')
      router.push('/')
      return true
    } catch (error) {
      console.error('Login error:', error)
      addNotification(error.response?.data?.message || 'خطا در ورود', 'error')
      return false
    } finally {
      loading.value = false
    }
  }

  const logout = () => {
    console.log('Logging out...')
    token.value = null
    user.value = null
    
    if (process.client) {
      localStorage.removeItem('auth-token')
      localStorage.removeItem('auth-user')
    }
    
    delete axios.defaults.headers.common['Authorization']
    router.push('/login')
    addNotification('با موفقیت خارج شدید', 'info')
  }

  const checkAuth = async () => {
    try {
      if (!token.value) return false
      const response = await axios.get('/api/auth/check')
      return true
    } catch (error) {
      console.error('Check auth error:', error)
      logout()
      return false
    }
  }

  // اجرای initializeAuth در زمان ایجاد store
  if (process.client) {
    initializeAuth()
  }

  return {
    user,
    token,
    loading,
    isAuthenticated,
    username,
    register,
    login,
    logout,
    checkAuth,
    initializeAuth
  }
}, {
  persist: {
    storage: localStorage,
    paths: ['user', 'token']
  }
}) 