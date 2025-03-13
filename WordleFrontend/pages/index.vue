<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <header class="bg-white dark:bg-gray-800 shadow">
      <div class="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center">
          <h1 class="text-3xl font-bold text-gray-900 dark:text-white">
            {{ t('gameTitle') }}
          </h1>
          <div class="flex items-center space-x-4">
            <select
              v-model="difficulty"
              class="bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
              @change="resetGame"
            >
              <option value="easy">{{ t('easy') }}</option>
              <option value="medium">{{ t('medium') }}</option>
              <option value="hard">{{ t('hard') }}</option>
            </select>
            <button
              @click="setLocale(locale === 'fa' ? 'en' : 'fa')"
              class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white"
            >
              {{ t('language') }}
            </button>
            <button
              @click="toggleTheme"
              class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white"
            >
              {{ t(theme === 'light' ? 'dark' : 'light') }}
            </button>
            <button
              @click="handleLogout"
              class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white"
            >
              {{ t('logout') }}
            </button>
          </div>
        </div>
      </div>
    </header>

    <main class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <div class="px-4 py-6 sm:px-0">
        <div class="flex flex-col items-center space-y-8">
          <div class="w-full max-w-md">
            <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
              <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-4">
                {{ t('guessWord') }}
              </h2>
              <div class="space-y-4">
                <div v-for="row in maxGuesses" :key="row" class="flex space-x-2">
                  <div
                    v-for="col in wordLength"
                    :key="col"
                    class="w-12 h-12 flex items-center justify-center text-2xl font-bold rounded border-2 cursor-pointer"
                    :class="[
                      getLetterClass(guesses[row - 1]?.[col - 1], col - 1, row),
                      currentRow === row - 1 && currentCol === col ? 'border-indigo-500 ring-2 ring-indigo-500' : '',
                      completedRows.value && completedRows.value.has(row - 1) ? 'flip-animation' : ''
                    ]"
                  >
                    {{ guesses[row - 1]?.[col - 1] || '' }}
                  </div>
                </div>
                <div v-if="gameOver" class="text-center">
                  <p class="text-xl font-bold mb-4">
                    {{ gameWon ? t('win') : t('lose') }}
                  </p>
                  <p class="mb-4">{{ t('correctWord') }}: {{ correctWord }}</p>
                  <button
                    @click="resetGame"
                    class="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-700"
                  >
                    {{ t('playAgain') }}
                  </button>
                </div>
                <div v-else class="flex flex-col space-y-4">
                  <div class="flex justify-between items-center">
                    <button
                      @click="useHelp"
                      class="bg-purple-600 text-white px-4 py-2 rounded hover:bg-purple-700 flex items-center space-x-2"
                    >
                      <span>{{ t('help') }}</span>
                      <span class="bg-white text-purple-600 rounded-full w-6 h-6 flex items-center justify-center">
                        {{ helpCount }}
                      </span>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="w-full max-w-md">
            <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
              <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-4">
                {{ t('score') }}
              </h2>
              <div class="text-center">
                <p class="text-3xl font-bold text-indigo-600 dark:text-indigo-400">
                  {{ score }}
                </p>
              </div>
            </div>
          </div>

          <div class="w-full max-w-md">
            <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
              <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-4">
                {{ t('statistics') }}
              </h2>
              <div class="grid grid-cols-2 gap-4">
                <div class="text-center">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ gamesPlayed }}
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('gamesPlayed') }}
                  </p>
                </div>
                <div class="text-center">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ winRate }}%
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('winRate') }}
                  </p>
                </div>
                <div class="text-center">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ currentStreak }}
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('currentStreak') }}
                  </p>
                </div>
                <div class="text-center">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ bestStreak }}
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('bestStreak') }}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import { useTranslations } from '../composables/useTranslations'
import { useRouter } from 'vue-router'
import { ref, computed, onMounted, onUnmounted } from 'vue'

const { t, locale, setLocale } = useTranslations()
const router = useRouter()
const theme = ref('light')
const difficulty = ref('medium')
const guesses = ref([])
const currentRow = ref(0)
const currentCol = ref(0)
const gameOver = ref(false)
const gameWon = ref(false)
const correctWord = ref('کتابخانه')
const completedRows = ref(new Set())
const gamesPlayed = ref(0)
const winRate = ref(0)
const currentStreak = ref(0)
const bestStreak = ref(0)
const wordLength = ref(8)
const score = ref(0)
const helpCount = ref(4)
const revealedLetters = ref([])

const maxGuesses = computed(() => {
  switch (difficulty.value) {
    case 'easy':
      return 8
    case 'medium':
      return 6
    case 'hard':
      return 4
    default:
      return 6
  }
})

const toggleTheme = () => {
  theme.value = theme.value === 'light' ? 'dark' : 'light'
  document.documentElement.classList.toggle('dark')
}

const useHelp = () => {
  if (helpCount.value <= 0) {
    alert(t('noHelpLeft'))
    return
  }

  // پیدا کردن یک حرف درست که هنوز نشان داده نشده
  const unrevealedIndices = Array.from({ length: wordLength.value }, (_, i) => i)
    .filter(i => !revealedLetters.value.includes(i))

  if (unrevealedIndices.length === 0) {
    alert(t('noHelpLeft'))
    return
  }

  const randomIndex = unrevealedIndices[Math.floor(Math.random() * unrevealedIndices.length)]
  revealedLetters.value.push(randomIndex)
  helpCount.value--
  alert(t('helpUsed'))
}

const getLetterClass = (letter, index, row) => {
  if (!letter) return 'border-gray-300 dark:border-gray-600'
  
  // اگر ردیف هنوز کامل نشده، فقط حاشیه خاکستری نشان بده
  if (!completedRows.value || !completedRows.value.has(row - 1)) {
    return 'border-gray-300 dark:border-gray-600'
  }
  
  // برای ردیف‌های کامل شده، رنگ‌ها را نشان بده
  if (letter === correctWord.value[index]) return 'bg-green-500 text-white border-green-500'
  if (correctWord.value.includes(letter)) return 'bg-yellow-500 text-white border-yellow-500'
  return 'bg-gray-500 text-white border-gray-500'
}

const calculateScore = (guessCount) => {
  const baseScore = {
    easy: 100,
    medium: 200,
    hard: 300
  }
  
  const remainingGuesses = maxGuesses.value - guessCount
  const bonus = remainingGuesses * 10
  
  return baseScore[difficulty.value] + bonus
}

const handleKeyPress = (event) => {
  if (gameOver.value) return

  if (event.key === 'Enter') {
    if (currentCol.value === wordLength.value) {
      submitGuess()
    }
  } else if (event.key === 'Backspace') {
    // فقط اگر ردیف فعلی هنوز کامل نشده، اجازه پاک کردن بده
    if ((!completedRows.value || !completedRows.value.has(currentRow.value)) && currentCol.value > 0) {
      currentCol.value--
      if (guesses.value[currentRow.value]) {
        guesses.value[currentRow.value] = guesses.value[currentRow.value].slice(0, -1)
      }
    }
  } else if (event.key.length === 1 && /^[\u0600-\u06FF]$/.test(event.key)) {
    // فقط اگر ردیف فعلی هنوز کامل نشده، اجازه تایپ بده
    if ((!completedRows.value || !completedRows.value.has(currentRow.value)) && currentCol.value < wordLength.value) {
      if (!guesses.value[currentRow.value]) {
        guesses.value[currentRow.value] = ''
      }
      guesses.value[currentRow.value] += event.key
      currentCol.value++
    }
  }
}

const submitGuess = () => {
  if (!guesses.value[currentRow.value] || guesses.value[currentRow.value].length !== wordLength.value) return
  
  // اضافه کردن ردیف فعلی به ردیف‌های کامل شده
  completedRows.value.add(currentRow.value)
  
  // صبر کردن برای اتمام انیمیشن
  setTimeout(() => {
    if (guesses.value[currentRow.value] === correctWord.value) {
      gameOver.value = true
      gameWon.value = true
      gamesPlayed.value++
      currentStreak.value++
      bestStreak.value = Math.max(bestStreak.value, currentStreak.value)
      winRate.value = Math.round((gamesPlayed.value / (gamesPlayed.value + 1)) * 100)
      score.value += calculateScore(currentRow.value + 1)
      helpCount.value++
    } else if (currentRow.value >= maxGuesses.value - 1) {
      gameOver.value = true
      gameWon.value = false
      gamesPlayed.value++
      currentStreak.value = 0
      winRate.value = Math.round((gamesPlayed.value / (gamesPlayed.value + 1)) * 100)
    } else {
      currentRow.value++
      currentCol.value = 0
    }
  }, 600) // زمان انیمیشن
}

const resetGame = () => {
  guesses.value = []
  currentRow.value = 0
  currentCol.value = 0
  gameOver.value = false
  gameWon.value = false
  revealedLetters.value = []
  completedRows.value.clear() // پاک کردن ردیف‌های کامل شده
  // TODO: Get new word based on difficulty
}

const handleLogout = () => {
  // TODO: Implement logout logic
  router.push('/login')
}

onMounted(() => {
  window.addEventListener('keydown', handleKeyPress)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyPress)
})
</script>

<style scoped>
@keyframes flip {
  0% {
    transform: rotateX(0deg);
  }
  50% {
    transform: rotateX(90deg);
  }
  100% {
    transform: rotateX(0deg);
  }
}

.flip-animation {
  animation: flip 0.6s ease-in-out;
  transform-style: preserve-3d;
  perspective: 1000px;
}
</style> 