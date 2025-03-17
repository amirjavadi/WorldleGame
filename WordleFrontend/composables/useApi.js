import { ref } from 'vue'

const baseURL = 'http://localhost:3001/api'

export const useApi = () => {
  const loading = ref(false)
  const error = ref(null)

  const request = async (endpoint, options = {}) => {
    loading.value = true
    error.value = null

    try {
      const response = await fetch(`${baseURL}${endpoint}`, {
        ...options,
        headers: {
          'Content-Type': 'application/json',
          ...options.headers,
        },
      })

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      const data = await response.json()
      return data
    } catch (e) {
      error.value = e.message
      throw e
    } finally {
      loading.value = false
    }
  }

  const get = (endpoint, options = {}) => {
    return request(endpoint, { ...options, method: 'GET' })
  }

  const post = (endpoint, body, options = {}) => {
    return request(endpoint, {
      ...options,
      method: 'POST',
      body: JSON.stringify(body),
    })
  }

  const put = (endpoint, body, options = {}) => {
    return request(endpoint, {
      ...options,
      method: 'PUT',
      body: JSON.stringify(body),
    })
  }

  const del = (endpoint, options = {}) => {
    return request(endpoint, { ...options, method: 'DELETE' })
  }

  return {
    loading,
    error,
    get,
    post,
    put,
    delete: del,
  }
} 