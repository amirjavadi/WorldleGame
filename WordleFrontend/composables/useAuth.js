import { ref } from 'vue'
import { useTranslations } from './useTranslations'

export const useAuth = () => {
  const { t } = useTranslations()
  const token = ref(process.client ? localStorage.getItem('token') : null)
  const user = ref(process.client ? JSON.parse(localStorage.getItem('user')) : null)
  const loading = ref(false)
  const error = ref(null)
  const isGuest = ref(process.client ? localStorage.getItem('isGuest') === 'true' : false)

  const setAuthData = (userData, authToken) => {
    if (process.client) {
      user.value = userData
      token.value = authToken
      localStorage.setItem('user', JSON.stringify(userData))
      localStorage.setItem('token', authToken)
      isGuest.value = false
      localStorage.setItem('isGuest', 'false')
    }
  }

  const clearAuthData = () => {
    if (process.client) {
      user.value = null
      token.value = null
      isGuest.value = false
      localStorage.removeItem('user')
      localStorage.removeItem('token')
      localStorage.removeItem('isGuest')
    }
  }

  const setGuestMode = () => {
    if (process.client) {
      isGuest.value = true
      localStorage.setItem('isGuest', 'true')
    }
  }

  const login = async (username, password) => {
    try {
      loading.value = true
      error.value = null
      
      // TODO: Replace with actual API call
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password }),
      })

      if (!response.ok) {
        throw new Error('Login failed')
      }

      const data = await response.json()
      setAuthData(data.user, data.token)
      return true
    } catch (e) {
      error.value = e.message
      return false
    } finally {
      loading.value = false
    }
  }

  const register = async (username, email, password) => {
    try {
      loading.value = true
      error.value = null

      // TODO: Replace with actual API call
      const response = await fetch('/api/auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, email, password }),
      })

      if (!response.ok) {
        throw new Error('Registration failed')
      }

      const data = await response.json()
      setAuthData(data.user, data.token)
      return true
    } catch (e) {
      error.value = e.message
      return false
    } finally {
      loading.value = false
    }
  }

  const logout = () => {
    clearAuthData()
  }

  const checkAuth = () => {
    return !!token.value || isGuest.value
  }

  return {
    user,
    token,
    loading,
    error,
    isGuest,
    login,
    register,
    logout,
    checkAuth,
    setGuestMode
  }
}

export default useAuth 