import { ref } from 'vue'
import { useApi } from './useApi'

export const useGame = () => {
  const word = ref('')
  const guesses = ref([])
  const currentGuess = ref('')
  const error = ref(null)
  const loading = ref(false)
  const isGameOver = ref(false)
  const isWon = ref(false)
  const api = useApi()

  const initGame = async () => {
    loading.value = true
    error.value = null
    try {
      const response = await api.game.getWord()
      word.value = response.word
      guesses.value = []
      currentGuess.value = ''
      isGameOver.value = false
      isWon.value = false
    } catch (e) {
      error.value = e.message || 'خطا در شروع بازی'
    } finally {
      loading.value = false
    }
  }

  const submitGuess = async () => {
    if (currentGuess.value.length !== 5) {
      error.value = 'کلمه باید ۵ حرفی باشد'
      return
    }

    loading.value = true
    error.value = null
    try {
      const response = await api.game.submitGuess(currentGuess.value)
      guesses.value.push({
        word: currentGuess.value,
        result: response.result
      })
      currentGuess.value = ''
      isWon.value = response.isCorrect
      isGameOver.value = isWon.value || guesses.value.length >= 6
    } catch (e) {
      error.value = e.message || 'خطا در ارسال حدس'
    } finally {
      loading.value = false
    }
  }

  return {
    word,
    guesses,
    currentGuess,
    error,
    loading,
    isGameOver,
    isWon,
    initGame,
    submitGuess
  }
} 