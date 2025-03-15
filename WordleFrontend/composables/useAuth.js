import { ref, onMounted } from 'vue'
import { useApi } from './useApi'

export const useAuth = () => {
  const user = ref(null)
  const loading = ref(false)
  const error = ref(null)
  const api = useApi()

  const initAuth = () => {
    const token = localStorage.getItem('token')
    const storedUser = localStorage.getItem('user')
    if (token && storedUser) {
      user.value = JSON.parse(storedUser)
    } else if (localStorage.getItem('isGuest')) {
      user.value = {
        username: 'مهمان',
        role: 'guest'
      }
    }
  }

  const checkAuth = () => {
    const token = localStorage.getItem('token')
    return !!token
  }

  const login = async (username, password) => {
    loading.value = true
    error.value = null
    try {
      const response = await api.auth.login(username, password)
      if (response.token) {
        localStorage.setItem('token', response.token)
        const userData = {
          username: username,
          role: response.role
        }
        user.value = userData
        localStorage.setItem('user', JSON.stringify(userData))
        return true
      }
      return false
    } catch (e) {
      error.value = e.message || 'خطا در ورود'
      return false
    } finally {
      loading.value = false
    }
  }

  const playAsGuest = () => {
    const userData = {
      username: 'مهمان',
      role: 'guest'
    }
    user.value = userData
    localStorage.setItem('isGuest', 'true')
    localStorage.setItem('user', JSON.stringify(userData))
    return true
  }

  const logout = () => {
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('isGuest')
    localStorage.removeItem('user')
  }

  const register = async (username, email, password) => {
    loading.value = true
    error.value = null
    try {
      const response = await api.auth.register(username, email, password)
      if (response.success) {
        return true
      }
      return false
    } catch (e) {
      error.value = e.message || 'خطا در ثبت نام'
      return false
    } finally {
      loading.value = false
    }
  }

  // Initialize auth state
  onMounted(() => {
    initAuth()
  })

  return {
    user,
    loading,
    error,
    login,
    logout,
    register,
    checkAuth,
    playAsGuest,
    initAuth
  }
} 