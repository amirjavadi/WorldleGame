import { defineStore } from 'pinia'

export const useGameStore = defineStore('game', {
  state: () => ({
    currentWord: null,
    guesses: [],
    attempts: 0,
    maxAttempts: 6,
    isWon: false,
    isGameOver: false,
    score: 0
  }),

  getters: {
    currentGuess: (state) => state.guesses[state.guesses.length - 1] || '',
    remainingAttempts: (state) => state.maxAttempts - state.attempts,
    isComplete: (state) => state.isWon || state.isGameOver
  },

  actions: {
    async fetchTodayWord() {
      try {
        const response = await $fetch(`${useRuntimeConfig().public.apiBase}/word/today`, {
          headers: {
            Authorization: `Bearer ${useAuthStore().authToken}`
          }
        })
        this.currentWord = response
        return response
      } catch (error) {
        throw error
      }
    },

    async makeGuess(guess) {
      try {
        const response = await $fetch(`${useRuntimeConfig().public.apiBase}/game/guess`, {
          method: 'POST',
          headers: {
            Authorization: `Bearer ${useAuthStore().authToken}`
          },
          body: { guess }
        })

        this.guesses.push(guess)
        this.attempts++
        this.isWon = response.isWon
        this.isGameOver = this.isWon || this.attempts >= this.maxAttempts
        this.score = response.score

        return response
      } catch (error) {
        throw error
      }
    },

    resetGame() {
      this.currentWord = null
      this.guesses = []
      this.attempts = 0
      this.isWon = false
      this.isGameOver = false
      this.score = 0
    }
  }
}) 