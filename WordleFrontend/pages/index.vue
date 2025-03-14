<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 dark:from-gray-900 dark:to-gray-800">
    <header class="bg-white/80 dark:bg-gray-800/80 backdrop-blur-sm shadow-lg">
      <div class="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center">
          <h1 class="text-3xl font-bold text-gray-900 dark:text-white font-display">
            {{ t('gameTitle') }}
          </h1>
          <div class="flex items-center space-x-4">
            <select
              v-model="difficulty"
              class="bg-white/80 dark:bg-gray-700/80 backdrop-blur-sm border border-gray-300 dark:border-gray-600 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
              @change="resetGame"
            >
              <option value="easy">{{ t('easy') }}</option>
              <option value="medium">{{ t('medium') }}</option>
              <option value="hard">{{ t('hard') }}</option>
            </select>
            <button
              @click="setLocale(locale === 'fa' ? 'en' : 'fa')"
              class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            >
              {{ t('language') }}
            </button>
            <button
              @click="toggleTheme"
              class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            >
              {{ t(theme === 'light' ? 'dark' : 'light') }}
            </button>
            <button
              @click="handleLogout"
              class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
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
            <div class="bg-white/80 dark:bg-gray-800/80 backdrop-blur-sm shadow-xl rounded-xl p-6 transform hover:scale-[1.02] transition-all duration-300">
              <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-4 font-display">
                {{ t('guessWord') }}
              </h2>
              <div class="space-y-4">
                <div v-for="row in maxGuesses" :key="row" class="flex space-x-2">
                  <div
                    v-for="col in wordLength"
                    :key="col"
                    class="w-12 h-12 flex items-center justify-center text-2xl font-bold rounded-lg border-2 cursor-pointer transition-all duration-200 hover:border-indigo-500 relative"
                    :class="[
                      getLetterClass(guesses[row - 1]?.[col - 1], col - 1, row),
                      currentRow === row - 1 && currentCol === col ? 'border-indigo-500 ring-4 ring-indigo-200 dark:ring-indigo-900 shadow-lg active-square' : '',
                      completedRows.value && completedRows.value.has(row - 1) ? 'flip-animation' : ''
                    ]"
                  >
                    {{ guesses[row - 1]?.[col - 1] || '' }}
                  </div>
                </div>
                <div v-if="gameOver" class="text-center">
                  <p class="text-xl font-bold mb-4 text-gray-900 dark:text-white">
                    {{ gameWon ? t('win') : t('lose') }}
                  </p>
                  <p class="mb-4 text-gray-700 dark:text-gray-300">{{ t('correctWord') }}: {{ correctWord }}</p>
                  <button
                    @click="resetGame"
                    class="bg-indigo-600 text-white px-6 py-3 rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200 shadow-lg"
                  >
                    {{ t('playAgain') }}
                  </button>
                </div>
                <div v-else class="flex flex-col space-y-4">
                  <div class="flex justify-between items-center">
                    <button
                      @click="useHelp"
                      class="bg-purple-600 text-white px-6 py-3 rounded-lg hover:bg-purple-700 flex items-center space-x-2 transform hover:scale-105 transition-all duration-200 shadow-lg"
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
            <div class="bg-white/80 dark:bg-gray-800/80 backdrop-blur-sm shadow-xl rounded-xl p-6 transform hover:scale-[1.02] transition-all duration-300">
              <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-4 font-display">
                {{ t('score') }}
              </h2>
              <div class="text-center">
                <p class="text-4xl font-bold text-indigo-600 dark:text-indigo-400">
                  {{ score }}
                </p>
              </div>
            </div>
          </div>

          <div class="w-full max-w-md">
            <div class="bg-white/80 dark:bg-gray-800/80 backdrop-blur-sm shadow-xl rounded-xl p-6 transform hover:scale-[1.02] transition-all duration-300">
              <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-4 font-display">
                {{ t('statistics') }}
              </h2>
              <div class="grid grid-cols-2 gap-4">
                <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ gamesPlayed }}
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('gamesPlayed') }}
                  </p>
                </div>
                <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ winRate }}%
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('winRate') }}
                  </p>
                </div>
                <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
                  <p class="text-2xl font-bold text-gray-900 dark:text-white">
                    {{ currentStreak }}
                  </p>
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t('currentStreak') }}
                  </p>
                </div>
                <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
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
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'

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

// اضافه کردن computed property برای جهت متن
const textDirection = computed(() => {
  return locale.value === 'fa' ? 'rtl' : 'ltr'
})

// اضافه کردن watch برای تغییر جهت متن
watch(locale, (newLocale) => {
  document.documentElement.dir = newLocale === 'fa' ? 'rtl' : 'ltr'
  document.documentElement.lang = newLocale
})

// اضافه کردن onMounted برای تنظیم جهت متن اولیه
onMounted(() => {
  document.documentElement.dir = locale.value === 'fa' ? 'rtl' : 'ltr'
  document.documentElement.lang = locale.value
})

onMounted(() => {
  window.addEventListener('keydown', handleKeyPress)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyPress)
})
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;500;600;700&display=swap');

.font-display {
  font-family: 'Vazirmatn', sans-serif;
}

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

/* اضافه کردن انیمیشن برای دکمه‌ها */
@keyframes pulse {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
  100% {
    transform: scale(1);
  }
}

button:hover {
  animation: pulse 1s infinite;
}

/* اضافه کردن انیمیشن برای مربع فعال */
@keyframes activeSquarePulse {
  0% {
    box-shadow: 0 0 0 0 rgba(99, 102, 241, 0.4);
  }
  70% {
    box-shadow: 0 0 0 10px rgba(99, 102, 241, 0);
  }
  100% {
    box-shadow: 0 0 0 0 rgba(99, 102, 241, 0);
  }
}

.active-square {
  animation: activeSquarePulse 2s infinite;
  border-width: 3px;
  transform: scale(1.05);
}

/* بهبود انیمیشن hover */
.w-12.h-12:hover {
  transform: scale(1.1);
  transition: transform 0.2s ease-in-out;
}

/* اضافه کردن افکت درخشان برای مربع‌های فعال */
.active-square::before {
  content: '';
  position: absolute;
  top: -2px;
  left: -2px;
  right: -2px;
  bottom: -2px;
  background: linear-gradient(45deg, rgba(99, 102, 241, 0.1), rgba(99, 102, 241, 0.3));
  border-radius: 0.5rem;
  z-index: -1;
  animation: glowPulse 2s infinite;
}

@keyframes glowPulse {
  0% {
    opacity: 0.5;
  }
  50% {
    opacity: 1;
  }
  100% {
    opacity: 0.5;
  }
}
</style> 