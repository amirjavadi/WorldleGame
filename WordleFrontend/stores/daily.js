import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useAuthStore } from './auth'
import { api } from '~/services/api'

export const useDailyStore = defineStore('daily', () => {
  const authStore = useAuthStore()
  
  // State
  const word = ref(null)
  const guesses = ref([])
  const loading = ref(false)
  const error = ref(null)
  const lastPlayedAt = ref(null)
  const timeUntilNextChallenge = ref(null)
  const participationId = ref(null)
  const stats = ref(null)

  // Getters
  const canPlay = computed(() => {
    if (!word.value) return false
    if (!lastPlayedAt.value) return true
    return new Date(lastPlayedAt.value).getDate() !== new Date().getDate()
  })

  const isGameOver = computed(() => {
    if (!guesses.value.length) return false
    const lastGuess = guesses.value[guesses.value.length - 1]
    return lastGuess?.isCorrect || guesses.value.length >= 6
  })

  // Actions
  const resetError = () => {
    error.value = null
  }

  const resetState = () => {
    word.value = null
    guesses.value = []
    loading.value = false
    error.value = null
    lastPlayedAt.value = null
    timeUntilNextChallenge.value = null
    participationId.value = null
    stats.value = null
  }

  const checkAuthAndToken = () => {
    if (!authStore.isLoggedIn || authStore.isGuest) {
      error.value = 'برای دسترسی به چالش روزانه باید وارد حساب کاربری خود شوید.'
      return false
    }

    if (!authStore.token) {
      error.value = 'توکن نامعتبر است. لطفاً دوباره وارد شوید.'
      return false
    }

    return true
  }

  const fetchDailyWord = async () => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      
      // دریافت چالش امروز
      const challenge = await api.daily.getWord(authStore.token)
      word.value = challenge.word

      // بررسی مشارکت امروز
      try {
        const participation = await api.daily.getMyParticipation(null, authStore.token)
        if (participation) {
          participationId.value = participation.id
          guesses.value = participation.guesses || []
          lastPlayedAt.value = participation.createdAt
          timeUntilNextChallenge.value = participation.timeUntilNext
          return
        }
      } catch (err) {
        if (err.message !== 'لطفاً دوباره وارد شوید') {
          throw err
        }
      }

      // شروع مشارکت جدید
      const newParticipation = await api.daily.participate(authStore.token)
      participationId.value = newParticipation.id
      guesses.value = []
      lastPlayedAt.value = null
    } catch (err) {
      console.error('Error fetching daily word:', err)
      error.value = err.message || 'خطا در دریافت کلمه روزانه'
      word.value = null
      if (err.message === 'لطفاً دوباره وارد شوید') {
        authStore.resetState()
      }
    } finally {
      loading.value = false
    }
  }

  const submitGuess = async (guess) => {
    if (!checkAuthAndToken()) return

    if (!participationId.value) {
      error.value = 'لطفاً ابتدا در چالش شرکت کنید.'
      return
    }

    if (isGameOver.value) {
      error.value = 'بازی به پایان رسیده است.'
      return
    }

    try {
      loading.value = true
      error.value = null

      const result = await api.daily.submitGuess(
        participationId.value,
        guess,
        authStore.token
      )

      guesses.value = result.guesses
      lastPlayedAt.value = result.updatedAt
      timeUntilNextChallenge.value = result.timeUntilNext

      if (result.isGameOver) {
        const leaderboard = await api.daily.getLeaderboard(null, 10, authStore.token)
        stats.value = leaderboard
      }

      return result
    } catch (err) {
      console.error('Error submitting guess:', err)
      error.value = err.message || 'خطا در ثبت حدس'
      if (err.message === 'لطفاً دوباره وارد شوید') {
        authStore.resetState()
      }
      return null
    } finally {
      loading.value = false
    }
  }

  return {
    // State
    word,
    guesses,
    loading,
    error,
    lastPlayedAt,
    timeUntilNextChallenge,
    stats,
    
    // Getters
    canPlay,
    isGameOver,
    
    // Actions
    resetError,
    resetState,
    fetchDailyWord,
    submitGuess
  }
}) 