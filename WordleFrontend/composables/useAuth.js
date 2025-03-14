import { ref } from 'vue'
import { useApi } from './useApi'

export const useAuth = () => {
  const user = ref(null)
  const loading = ref(false)
  const error = ref(null)
  const api = useApi()

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
        user.value = {
          username: username,
          role: response.role
        }
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
    user.value = {
      username: 'مهمان',
      role: 'guest'
    }
    localStorage.setItem('isGuest', 'true')
    return true
  }

  const logout = () => {
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('isGuest')
  }

  const register = async (username, email, password) => {
    loading.value = true
    error.value = null
    try {
      const response = await api.auth.register(username, email, password)
      if (response.success) {
        return await login(username, password)
      }
      return false
    } catch (e) {
      error.value = e.message || 'خطا در ثبت نام'
      return false
    } finally {
      loading.value = false
    }
  }

  return {
    user,
    loading,
    error,
    login,
    logout,
    register,
    checkAuth,
    playAsGuest
  }
} 