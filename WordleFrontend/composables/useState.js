import { ref } from 'vue'

export const useLocalStorage = () => {
  const getItem = (key) => {
    if (process.client) {
      const item = localStorage.getItem(key)
      try {
        return item ? JSON.parse(item) : null
      } catch {
        return item
      }
    }
    return null
  }

  const setItem = (key, value) => {
    if (process.client) {
      localStorage.setItem(key, JSON.stringify(value))
    }
  }

  const removeItem = (key) => {
    if (process.client) {
      localStorage.removeItem(key)
    }
  }

  return {
    getItem,
    setItem,
    removeItem
  }
} 