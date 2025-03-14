import { useRuntimeConfig } from '#app'

export const useApi = () => {
  const config = useRuntimeConfig()
  const baseURL = config.public.apiBase

  const handleResponse = async (response) => {
    if (!response.ok) {
      const error = await response.json()
      throw new Error(error.message || 'خطای سرور')
    }
    return response.json()
  }

  const auth = {
    login: async (username, password) => {
      const response = await fetch(`${baseURL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
      })
      return handleResponse(response)
    },

    register: async (username, email, password) => {
      const response = await fetch(`${baseURL}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, email, password })
      })
      return handleResponse(response)
    }
  }

  const game = {
    getWord: async () => {
      const response = await fetch(`${baseURL}/game/word`)
      return handleResponse(response)
    },

    submitGuess: async (guess) => {
      const token = localStorage.getItem('token')
      const response = await fetch(`${baseURL}/game/guess`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ guess })
      })
      return handleResponse(response)
    },

    getStats: async () => {
      const token = localStorage.getItem('token')
      const response = await fetch(`${baseURL}/game/stats`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      return handleResponse(response)
    }
  }

  const leaderboard = {
    getTop: async () => {
      const response = await fetch(`${baseURL}/leaderboard`)
      return handleResponse(response)
    }
  }

  const admin = {
    addWord: async (word) => {
      const token = localStorage.getItem('token')
      const response = await fetch(`${baseURL}/admin/words`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ word })
      })
      return handleResponse(response)
    },

    getWords: async () => {
      const token = localStorage.getItem('token')
      const response = await fetch(`${baseURL}/admin/words`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      return handleResponse(response)
    }
  }

  return {
    auth,
    game,
    leaderboard,
    admin
  }
} 