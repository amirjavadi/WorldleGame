import { ref } from 'vue'
import { useApi } from './useApi'

export const useAuth = () => {
  const api = useApi()
  const user = ref(null)
  const loading = ref(false)
  const error = ref(null)

  const login = async (username, password) => {
    loading.value = true
    error.value = null

    try {
      const response = await api.post('/auth/login', { username, password })
      user.value = response.user
      return response
    } catch (e) {
      error.value = e.message
      throw e
    } finally {
      loading.value = false
    }
  }

  const register = async (username, password, email) => {
    loading.value = true
    error.value = null

    try {
      const response = await api.post('/auth/register', { username, password, email })
      user.value = response.user
      return response
    } catch (e) {
      error.value = e.message
      throw e
    } finally {
      loading.value = false
    }
  }

  const logout = async () => {
    loading.value = true
    error.value = null

    try {
      await api.post('/auth/logout')
      user.value = null
    } catch (e) {
      error.value = e.message
      throw e
    } finally {
      loading.value = false
    }
  }

  const checkAuth = async () => {
    loading.value = true
    error.value = null

    try {
      const response = await api.get('/auth/me')
      user.value = response.user
      return response
    } catch (e) {
      error.value = e.message
      user.value = null
      throw e
    } finally {
      loading.value = false
    }
  }

  return {
    user,
    loading,
    error,
    login,
    register,
    logout,
    checkAuth,
  }
} 