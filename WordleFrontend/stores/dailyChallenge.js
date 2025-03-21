import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useAuthStore } from './auth'
import { api } from '~/services/api'

export const useDailyChallengeStore = defineStore('dailyChallenge', () => {
  const authStore = useAuthStore()
  
  // State
  const loading = ref(false)
  const error = ref(null)
  const word = ref(null)
  const guesses = ref([])
  const participationId = ref(null)
  const currentScore = ref(0)
  const personalBest = ref(0)
  const streak = ref(0)
  const winRate = ref(0)
  const leaderboard = ref([])
  const lastPlayedAt = ref(null)
  const timeUntilNextChallenge = ref(null)
  const stats = ref(null)
  const pastChallenges = ref([])
  let timerInterval = null

  // Computed
  const guessCount = computed(() => guesses.value.length)
  const hasWon = computed(() => {
    if (!word.value || guesses.value.length === 0) return false
    return guesses.value[guesses.value.length - 1] === word.value
  })
  const isCompleted = computed(() => {
    return hasWon.value || guessCount.value >= 6
  })
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
    stopTimer()
    word.value = null
    guesses.value = []
    loading.value = false
    error.value = null
    lastPlayedAt.value = null
    timeUntilNextChallenge.value = null
    participationId.value = null
    stats.value = null
    currentScore.value = 0
    personalBest.value = 0
    streak.value = 0
    winRate.value = 0
    leaderboard.value = []
    pastChallenges.value = []
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

  const startTimer = () => {
    if (timerInterval) {
      clearInterval(timerInterval)
    }

    // Reset the timer value
    if (timeUntilNextChallenge.value <= 0) {
      timeUntilNextChallenge.value = 24 * 60 * 60 * 1000 // 24 hours in milliseconds
    }

    timerInterval = setInterval(() => {
      if (timeUntilNextChallenge.value > 0) {
        timeUntilNextChallenge.value -= 1000
      } else {
        clearInterval(timerInterval)
        timerInterval = null
        // Refresh the challenge data
        fetchTodaysChallenge()
      }
    }, 1000)
  }

  const stopTimer = () => {
    if (timerInterval) {
      clearInterval(timerInterval)
      timerInterval = null
    }
  }

  const fetchTodaysChallenge = async () => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      
      // دریافت چالش امروز
      const challenge = await api.daily.getTodaysChallenge()
      word.value = challenge.word
      currentScore.value = challenge.score || 0
      personalBest.value = challenge.personalBest || 0
      streak.value = challenge.streak || 0
      winRate.value = challenge.winRate || 0

      // بررسی مشارکت امروز
      try {
        const response = await api.daily.getParticipation()
        if (response.hasParticipated && response.participation) {
          participationId.value = response.participation.id
          guesses.value = response.participation.guesses || []
          lastPlayedAt.value = response.participation.createdAt
          timeUntilNextChallenge.value = response.timeUntilNext
          startTimer() // Start the timer when we get the time
          return
        }
      } catch (err) {
        if (err.message !== 'لطفاً دوباره وارد شوید') {
          throw err
        }
      }

      // شروع مشارکت جدید
      const newParticipation = await api.daily.participate()
      participationId.value = newParticipation.id
      guesses.value = []
      lastPlayedAt.value = null
      timeUntilNextChallenge.value = 24 * 60 * 60 * 1000 // 24 hours in milliseconds
      startTimer() // Start the timer for new participation
    } catch (err) {
      console.error('Error fetching daily challenge:', err)
      error.value = err.message || 'خطا در دریافت چالش روزانه'
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

      const result = await api.daily.submitGuess(participationId.value, guess)
      guesses.value = result.guesses
      currentScore.value = result.score
      lastPlayedAt.value = result.updatedAt
      timeUntilNextChallenge.value = result.timeUntilNext

      if (result.isGameOver) {
        const leaderboard = await api.daily.getLeaderboard()
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

  const fetchLeaderboard = async (date = null, limit = 10) => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      const response = await api.daily.getLeaderboard(date, limit)
      leaderboard.value = response
    } catch (err) {
      console.error('Error fetching leaderboard:', err)
      error.value = err.message || 'خطا در دریافت جدول امتیازات'
    } finally {
      loading.value = false
    }
  }

  const fetchPastChallenges = async (days = 7) => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      const response = await api.daily.getPastChallenges(days)
      pastChallenges.value = response
    } catch (err) {
      console.error('Error fetching past challenges:', err)
      error.value = err.message || 'خطا در دریافت چالش‌های گذشته'
    } finally {
      loading.value = false
    }
  }

  const fetchDailyStats = async (date = null) => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      const response = await api.daily.getDailyStats(date)
      stats.value = response
    } catch (err) {
      console.error('Error fetching daily stats:', err)
      error.value = err.message || 'خطا در دریافت آمار روزانه'
    } finally {
      loading.value = false
    }
  }

  const fetchUserRank = async (date = null) => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      const response = await api.daily.getUserRank(date)
      return response
    } catch (err) {
      console.error('Error fetching user rank:', err)
      error.value = err.message || 'خطا در دریافت رتبه کاربر'
      return null
    } finally {
      loading.value = false
    }
  }

  const fetchUserParticipation = async () => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      const response = await api.daily.getParticipation()
      if (response.hasParticipated && response.participation) {
        participationId.value = response.participation.id
        guesses.value = response.participation.guesses || []
        lastPlayedAt.value = response.participation.createdAt
        timeUntilNextChallenge.value = response.timeUntilNext
      }
      return response
    } catch (err) {
      console.error('Error fetching user participation:', err)
      error.value = err.message || 'خطا در دریافت اطلاعات مشارکت'
      return null
    } finally {
      loading.value = false
    }
  }

  const startParticipation = async () => {
    if (!checkAuthAndToken()) return

    try {
      loading.value = true
      error.value = null
      const response = await api.daily.participate()
      participationId.value = response.id
      guesses.value = []
      lastPlayedAt.value = null
      return response
    } catch (err) {
      console.error('Error starting participation:', err)
      error.value = err.message || 'خطا در شروع مشارکت'
      return null
    } finally {
      loading.value = false
    }
  }

  return {
    // State
    loading,
    error,
    word,
    guesses,
    participationId,
    currentScore,
    personalBest,
    streak,
    winRate,
    leaderboard,
    lastPlayedAt,
    timeUntilNextChallenge,
    stats,
    pastChallenges,

    // Computed
    guessCount,
    hasWon,
    isCompleted,
    canPlay,
    isGameOver,

    // Actions
    resetError,
    resetState,
    fetchTodaysChallenge,
    submitGuess,
    fetchLeaderboard,
    fetchPastChallenges,
    fetchDailyStats,
    fetchUserRank,
    fetchUserParticipation,
    startParticipation,
    startTimer,
    stopTimer
  }
}, {
  persist: {
    key: 'daily-challenge-store',
    paths: [
      'word',
      'guesses',
      'participationId',
      'currentScore',
      'personalBest',
      'streak',
      'winRate',
      'lastPlayedAt',
      'timeUntilNextChallenge',
      'stats',
      'pastChallenges'
    ]
  }
}) 