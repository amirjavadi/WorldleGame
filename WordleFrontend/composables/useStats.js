import { ref } from 'vue'
import { useApi } from './useApi'

export const useStats = () => {
  const stats = ref(null)
  const loading = ref(false)
  const error = ref(null)
  const api = useApi()

  const fetchStats = async () => {
    loading.value = true
    error.value = null
    try {
      const response = await api.game.getStats()
      stats.value = response
    } catch (e) {
      error.value = e.message || 'خطا در دریافت آمار'
    } finally {
      loading.value = false
    }
  }

  return {
    stats,
    loading,
    error,
    fetchStats
  }
} 