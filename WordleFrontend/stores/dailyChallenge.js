import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { api } from '~/services/api'

export const useDailyChallengeStore = defineStore('dailyChallenge', () => {
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

  // Computed
  const guessCount = computed(() => guesses.value.length)
  const hasWon = computed(() => {
    if (!word.value || guesses.value.length === 0) return false
    return guesses.value[guesses.value.length - 1] === word.value
  })
  const isCompleted = computed(() => {
    return hasWon.value || guessCount.value >= 6
  })

  // Actions
  const fetchTodaysChallenge = async () => {
    try {
      loading.value = true
      error.value = null
      const response = await api.getTodaysChallenge()
      word.value = response.word
      guesses.value = response.guesses || []
      participationId.value = response.participationId
      currentScore.value = response.score || 0
      personalBest.value = response.personalBest || 0
      streak.value = response.streak || 0
      winRate.value = response.winRate || 0
      leaderboard.value = response.leaderboard || []
    } catch (err) {
      error.value = err.message
      console.error('Error fetching daily challenge:', err)
    } finally {
      loading.value = false
    }
  }

  const startParticipation = async () => {
    try {
      loading.value = true
      error.value = null
      const response = await api.participate()
      participationId.value = response.participationId
    } catch (err) {
      error.value = err.message
      console.error('Error starting participation:', err)
    } finally {
      loading.value = false
    }
  }

  const submitGuess = async (guess) => {
    try {
      loading.value = true
      error.value = null
      const response = await api.submitGuess(participationId.value, guess)
      guesses.value = response.guesses
      currentScore.value = response.score
      if (response.leaderboard) {
        leaderboard.value = response.leaderboard
      }
    } catch (err) {
      error.value = err.message
      console.error('Error submitting guess:', err)
    } finally {
      loading.value = false
    }
  }

  const fetchLeaderboard = async (date = null, limit = 10) => {
    try {
      loading.value = true
      error.value = null
      const response = await api.getLeaderboard(date, limit)
      leaderboard.value = response
    } catch (err) {
      error.value = err.message
      console.error('Error fetching leaderboard:', err)
    } finally {
      loading.value = false
    }
  }

  const checkParticipation = async (date = null) => {
    try {
      loading.value = true
      error.value = null
      const response = await api.getParticipation(date)
      return response
    } catch (err) {
      error.value = err.message
      console.error('Error checking participation:', err)
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

    // Computed
    guessCount,
    hasWon,
    isCompleted,

    // Actions
    fetchTodaysChallenge,
    startParticipation,
    submitGuess,
    fetchLeaderboard,
    checkParticipation
  }
}) 