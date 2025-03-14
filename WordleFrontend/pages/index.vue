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
              class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            >
              <i class="fas fa-language text-xl"></i>
            </button>
            <button
              @click="toggleTheme"
              class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            >
              <i :class="['fas', theme === 'light' ? 'fa-moon' : 'fa-sun', 'text-xl']"></i>
            </button>
            <button
              @click="showStats = true"
              class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            >
              <i class="fas fa-chart-bar text-xl"></i>
            </button>
            <div class="flex items-center space-x-2 bg-gradient-to-r from-indigo-100 to-purple-100 dark:from-indigo-900 dark:to-purple-900 px-4 py-2 rounded-lg shadow-sm">
              <i class="fas fa-trophy text-yellow-500 text-xl"></i>
              <span class="text-lg font-bold text-indigo-700 dark:text-indigo-300">{{ score }}</span>
              <span class="text-sm font-medium text-indigo-600 dark:text-indigo-400">{{ t('score') }}</span>
            </div>
            <div class="flex items-center space-x-2">
              <i class="fas fa-user-circle text-xl text-gray-600 dark:text-gray-300"></i>
              <span class="text-gray-900 dark:text-white font-medium">{{ username }}</span>
            </div>
            <button
              @click="handleLogout"
              class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            >
              <i class="fas fa-sign-out-alt text-xl"></i>
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
              <div class="flex justify-between items-center mb-4">
                <h2 class="text-xl font-semibold text-gray-900 dark:text-white font-display">
                  {{ t('guessWord') }}
                </h2>
                <!-- دکمه کمک -->
                <button
                  @click="useHelp"
                  class="relative group"
                >
                  <i class="fas fa-lightbulb text-2xl text-yellow-500 hover:text-yellow-600 transition-colors duration-200 animate-pulse"></i>
                  <span class="absolute -top-2 -right-2 bg-purple-600 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                    {{ helpCount }}
                  </span>
                </button>
              </div>
              <div class="space-y-4">
                <div v-for="row in maxGuesses" :key="row" class="flex space-x-2">
                  <div
                    v-for="col in wordLength"
                    :key="col"
                    class="w-12 h-12 flex items-center justify-center text-2xl font-bold rounded-lg border-2 cursor-pointer transition-all duration-200 hover:border-indigo-500 relative"
                    :class="[
                      getLetterClass(guesses[row - 1]?.[col - 1], col - 1, row),
                      currentRow === row - 1 && currentCol === col ? 'border-indigo-500 ring-4 ring-indigo-200 dark:ring-indigo-900 shadow-lg active-square cursor-text' : '',
                      currentRow === row - 1 && col === 0 && !guesses[row - 1]?.length ? 'initial-active' : '',
                      completedRows.value && completedRows.value.has(row - 1) ? 'flip-animation' : '',
                      currentRow === row - 1 ? 'active-row' : 'inactive-row'
                    ]"
                  >
                    {{ guesses[row - 1]?.[col - 1] || '' }}
                    <div v-if="(currentRow === row - 1 && currentCol === col) || (currentRow === row - 1 && col === 0 && !guesses[row - 1]?.length)" 
                         class="absolute bottom-1 left-1/2 transform -translate-x-1/2 w-1 h-1 bg-indigo-500 rounded-full cursor-blink">
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- مدال آمار -->
    <Teleport to="body">
      <div v-if="showStats || (gameOver && showGameOverStats)" 
           class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
        <div class="bg-white dark:bg-gray-800 rounded-xl p-8 max-w-md w-full mx-4 transform transition-all duration-300 scale-100">
          <div class="flex justify-between items-center mb-6">
            <h3 class="text-2xl font-bold text-gray-900 dark:text-white">{{ t('statistics') }}</h3>
            <button @click="closeStats" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
              <i class="fas fa-times text-xl"></i>
            </button>
          </div>
          
          <div class="grid grid-cols-2 gap-4 mb-6">
            <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
              <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ gamesPlayed }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('gamesPlayed') }}</p>
            </div>
            <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
              <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ winRate }}%</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('winRate') }}</p>
            </div>
            <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
              <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ currentStreak }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('currentStreak') }}</p>
            </div>
            <div class="text-center p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
              <p class="text-2xl font-bold text-gray-900 dark:text-white">{{ bestStreak }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ t('bestStreak') }}</p>
            </div>
          </div>

          <div v-if="gameOver" class="text-center">
            <button
              @click="resetGame(); closeStats();"
              class="bg-indigo-600 text-white px-6 py-3 rounded-lg hover:bg-indigo-700 transform hover:scale-105 transition-all duration-200 shadow-lg"
            >
              {{ t('playAgain') }}
            </button>
          </div>
        </div>
      </div>
    </Teleport>

    <!-- کیبورد -->
    <div class="fixed bottom-0 left-0 right-0 bg-white/95 dark:bg-gray-800/95 backdrop-blur-sm shadow-lg p-4">
      <div class="max-w-2xl mx-auto">
        <template v-if="locale === 'fa'">
          <div class="grid grid-cols-11 gap-1 mb-2">
            <button
              v-for="letter in 'ضصثقفغعهخحج'"
              :key="letter"
              @click="handleKeyPress({ key: letter })"
              class="p-2 text-center rounded-lg transition-all duration-200"
              :class="[
                getKeyboardLetterClass(letter),
                isLetterDisabled(letter) ? 'opacity-90 cursor-not-allowed bg-[#464b514d] text-[#646971] dark:bg-gray-600 dark:text-gray-400' : 'bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600'
              ]"
              :disabled="isLetterDisabled(letter) || gameOver"
            >
              {{ letter }}
            </button>
          </div>
          <div class="grid grid-cols-10 gap-1 mb-2">
            <button
              v-for="letter in 'شسیبلاتنمک'"
              :key="letter"
              @click="handleKeyPress({ key: letter })"
              class="p-2 text-center rounded-lg transition-all duration-200"
              :class="[
                getKeyboardLetterClass(letter),
                isLetterDisabled(letter) ? 'opacity-90 cursor-not-allowed bg-[#464b514d] text-[#646971] dark:bg-gray-600 dark:text-gray-400' : 'bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600'
              ]"
              :disabled="isLetterDisabled(letter) || gameOver"
            >
              {{ letter }}
            </button>
          </div>
          <div class="grid grid-cols-9 gap-1">
            <button
              v-for="letter in 'ظطزرذدپو'"
              :key="letter"
              @click="handleKeyPress({ key: letter })"
              class="p-2 text-center rounded-lg transition-all duration-200"
              :class="[
                getKeyboardLetterClass(letter),
                isLetterDisabled(letter) ? 'opacity-90 cursor-not-allowed bg-[#464b514d] text-[#646971] dark:bg-gray-600 dark:text-gray-400' : 'bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600'
              ]"
              :disabled="isLetterDisabled(letter) || gameOver"
            >
              {{ letter }}
            </button>
            <button
              @click="handleKeyPress({ key: 'Backspace' })"
              class="p-2 text-center rounded-lg bg-red-100 dark:bg-red-900 hover:bg-red-200 dark:hover:bg-red-800 transition-all duration-200"
              :disabled="gameOver"
            >
              <i class="fas fa-backspace"></i>
            </button>
          </div>
        </template>
        <template v-else>
          <div class="grid grid-cols-10 gap-1 mb-2">
            <button
              v-for="letter in 'QWERTYUIOP'"
              :key="letter"
              @click="handleKeyPress({ key: letter.toLowerCase() })"
              class="p-2 text-center rounded-lg transition-colors duration-200"
              :class="[
                getKeyboardLetterClass(letter.toLowerCase()),
                isLetterDisabled(letter.toLowerCase()) ? 'opacity-50 cursor-not-allowed' : 'bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600'
              ]"
              :disabled="isLetterDisabled(letter.toLowerCase()) || gameOver"
            >
              {{ letter }}
            </button>
          </div>
          <div class="grid grid-cols-9 gap-1 mb-2">
            <button
              v-for="letter in 'ASDFGHJKL'"
              :key="letter"
              @click="handleKeyPress({ key: letter.toLowerCase() })"
              class="p-2 text-center rounded-lg transition-colors duration-200"
              :class="[
                getKeyboardLetterClass(letter.toLowerCase()),
                isLetterDisabled(letter.toLowerCase()) ? 'opacity-50 cursor-not-allowed' : 'bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600'
              ]"
              :disabled="isLetterDisabled(letter.toLowerCase()) || gameOver"
            >
              {{ letter }}
            </button>
          </div>
          <div class="grid grid-cols-8 gap-1">
            <button
              v-for="letter in 'ZXCVBNM'"
              :key="letter"
              @click="handleKeyPress({ key: letter.toLowerCase() })"
              class="p-2 text-center rounded-lg transition-colors duration-200"
              :class="[
                getKeyboardLetterClass(letter.toLowerCase()),
                isLetterDisabled(letter.toLowerCase()) ? 'opacity-50 cursor-not-allowed' : 'bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600'
              ]"
              :disabled="isLetterDisabled(letter.toLowerCase()) || gameOver"
            >
              {{ letter }}
            </button>
            <button
              @click="handleKeyPress({ key: 'Backspace' })"
              class="p-2 text-center rounded-lg bg-red-100 dark:bg-red-900 hover:bg-red-200 dark:hover:bg-red-800 transition-colors duration-200"
              :disabled="gameOver"
            >
              <i class="fas fa-backspace"></i>
            </button>
          </div>
        </template>
        <div class="mt-2">
          <button
            @click="handleKeyPress({ key: 'Enter' })"
            class="w-full p-2 text-center rounded-lg bg-green-100 dark:bg-green-900 hover:bg-green-200 dark:hover:bg-green-800 transition-colors duration-200"
            :disabled="gameOver"
          >
            <i class="fas fa-check mr-2"></i>{{ t('submit') }}
          </button>
        </div>
      </div>
    </div>
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
const showStats = ref(false)
const showGameOverStats = ref(false)
const username = ref('کاربر') // این مقدار باید از سیستم احراز هویت دریافت شود
const disabledLetters = ref(new Set())
const helpLetters = ref(new Set()) // برای ذخیره موقعیت حروف کمکی

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

const getLetterClass = (letter, index, row) => {
  if (!letter) return 'border-gray-300 dark:border-gray-600'
  
  // اگر این حرف کمکی است، همیشه سبز نشان داده شود
  if (helpLetters.value.has(`${row-1}-${index}`)) {
    return 'bg-green-500 text-white border-green-500'
  }
  
  // اگر ردیف هنوز کامل نشده، فقط حاشیه خاکستری نشان بده
  if (!completedRows.value || !completedRows.value.has(row - 1)) {
    return 'border-gray-300 dark:border-gray-600'
  }
  
  // برای ردیف‌های کامل شده، رنگ‌ها را نشان بده
  if (letter === correctWord.value[index]) return 'bg-green-500 text-white border-green-500'
  if (correctWord.value.includes(letter)) return 'bg-yellow-500 text-white border-yellow-500'
  return 'bg-gray-500 text-white border-gray-500'
}

const findNextEmptyPosition = () => {
  for (let i = 0; i < wordLength.value; i++) {
    if (!guesses.value[currentRow.value]?.[i] && !helpLetters.value.has(`${currentRow.value}-${i}`)) {
      return i
    }
  }
  return -1
}

const useHelp = () => {
  if (helpCount.value <= 0) {
    alert(t('noHelpLeft'))
    return
  }

  // پیدا کردن موقعیت‌های خالی در سطر فعلی
  const emptyPositions = []
  for (let i = 0; i < wordLength.value; i++) {
    if (!guesses.value[currentRow.value]?.[i] && !helpLetters.value.has(`${currentRow.value}-${i}`)) {
      emptyPositions.push(i)
    }
  }

  if (emptyPositions.length === 0) {
    alert(t('rowIsFull'))
    return
  }

  // انتخاب یک موقعیت تصادفی از موقعیت‌های خالی
  const randomEmptyPosition = emptyPositions[Math.floor(Math.random() * emptyPositions.length)]

  // قرار دادن حرف درست در موقعیت انتخاب شده
  if (!guesses.value[currentRow.value]) {
    guesses.value[currentRow.value] = []
  }
  guesses.value[currentRow.value][randomEmptyPosition] = correctWord.value[randomEmptyPosition]
  helpLetters.value.add(`${currentRow.value}-${randomEmptyPosition}`)

  // انتقال فوکوس به اولین مربع خالی
  const nextEmpty = findNextEmptyPosition()
  currentCol.value = nextEmpty !== -1 ? nextEmpty : randomEmptyPosition + 1

  helpCount.value--
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

  const key = event.key?.toLowerCase() || event.toLowerCase()
  
  if (key === 'backspace' || key === 'enter') {
    if (key === 'backspace') {
      if (currentCol.value > 0) {
        // اگر حرف قبلی کمکی نیست، آن را پاک کن
        const prevPosition = `${currentRow.value}-${currentCol.value - 1}`
        if (!helpLetters.value.has(prevPosition)) {
          currentCol.value--
          guesses.value[currentRow.value][currentCol.value] = ''
        }
      }
    } else if (key === 'enter') {
      if (currentCol.value === wordLength.value) {
        submitGuess()
        updateDisabledLetters()
      }
    }
  } else {
    // بررسی اینکه آیا حرف معتبر است
    const validLetters = locale.value === 'fa' 
      ? 'ضصثقفغعهخحجشسیبلاتنمکظطزرذدپو'
      : 'abcdefghijklmnopqrstuvwxyz'
      
    if (validLetters.includes(key) && currentCol.value < wordLength.value && !isLetterDisabled(key)) {
      // اگر موقعیت فعلی حرف کمکی است، به دنبال موقعیت خالی بعدی برو
      const currentPosition = `${currentRow.value}-${currentCol.value}`
      if (helpLetters.value.has(currentPosition)) {
        const nextEmpty = findNextEmptyPosition()
        if (nextEmpty !== -1) {
          currentCol.value = nextEmpty
        }
      }

      if (!helpLetters.value.has(`${currentRow.value}-${currentCol.value}`)) {
        if (!guesses.value[currentRow.value]) {
          guesses.value[currentRow.value] = []
        }
        guesses.value[currentRow.value][currentCol.value] = key
        currentCol.value++
      }
    }
  }
}

const submitGuess = () => {
  if (!guesses.value[currentRow.value] || guesses.value[currentRow.value].length !== wordLength.value) return
  
  completedRows.value.add(currentRow.value)
  
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
      showGameOverStats.value = true
    } else if (currentRow.value >= maxGuesses.value - 1) {
      gameOver.value = true
      gameWon.value = false
      gamesPlayed.value++
      currentStreak.value = 0
      winRate.value = Math.round((gamesPlayed.value / (gamesPlayed.value + 1)) * 100)
      showGameOverStats.value = true
    } else {
      currentRow.value++
      currentCol.value = 0
    }
  }, 600)
}

const resetGame = () => {
  guesses.value = []
  currentRow.value = 0
  currentCol.value = 0
  gameOver.value = false
  gameWon.value = false
  revealedLetters.value = []
  completedRows.value.clear()
  disabledLetters.value.clear()
  helpLetters.value.clear() // پاک کردن حروف کمکی
  // TODO: Get new word based on difficulty
}

const handleLogout = () => {
  // TODO: Implement logout logic
  router.push('/login')
}

const closeStats = () => {
  showStats.value = false
  showGameOverStats.value = false
}

// اضافه کردن computed property برای جهت متن
const textDirection = computed(() => {
  return locale.value === 'fa' ? 'rtl' : 'ltr'
})

// اضافه کردن تابع برای بررسی غیرفعال بودن حروف
const isLetterDisabled = (letter) => {
  if (!letter) return false
  
  // فقط حروفی که در حدس‌های قبلی استفاده شده‌اند و در کلمه صحیح نیستند، غیرفعال می‌شوند
  for (let i = 0; i < currentRow.value; i++) {
    if (guesses.value[i] && guesses.value[i].includes(letter.toLowerCase()) && !correctWord.value.includes(letter.toLowerCase())) {
      return true
    }
  }
  return false
}

const updateDisabledLetters = () => {
  if (currentRow.value > 0) {
    const lastGuess = guesses.value[currentRow.value - 1]
    if (lastGuess) {
      lastGuess.forEach((letter, index) => {
        if (!correctWord.value.includes(letter.toLowerCase())) {
          disabledLetters.value.add(letter.toLowerCase())
        }
      })
    }
  }
}

// تغییر watch برای تغییر جهت متن و زبان
watch(locale, (newLocale) => {
  document.documentElement.dir = newLocale === 'fa' ? 'rtl' : 'ltr'
  document.documentElement.lang = newLocale
  document.body.style.textAlign = newLocale === 'fa' ? 'right' : 'left'
})

// اضافه کردن onMounted برای تنظیم جهت متن اولیه
onMounted(() => {
  document.documentElement.dir = locale.value === 'fa' ? 'rtl' : 'ltr'
  document.documentElement.lang = locale.value
  document.body.style.textAlign = locale.value === 'fa' ? 'right' : 'left'
})

onMounted(() => {
  window.addEventListener('keydown', handleKeyPress)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyPress)
})

// اضافه کردن تابع جدید برای کلاس‌های کیبورد
const getKeyboardLetterClass = (letter) => {
  let classes = 'text-gray-900 dark:text-white'
  
  if (isLetterDisabled(letter)) {
    classes = 'text-[#646971] dark:text-gray-400 bg-[#464b514d] dark:bg-gray-600'
  } else if (correctWord.value.includes(letter)) {
    classes += ' bg-green-500 text-white'
  } else if (guesses.value[currentRow.value]?.includes(letter)) {
    classes += ' bg-yellow-500 text-white'
  }
  
  return classes
}
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;500;600;700&display=swap');
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css');

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
  background-color: rgba(99, 102, 241, 0.05);
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
    opacity: 0.3;
  }
  50% {
    opacity: 0.6;
  }
  100% {
    opacity: 0.3;
  }
}

/* اضافه کردن انیمیشن چشمک زن برای نشانگر */
@keyframes blink {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0;
  }
}

.cursor-blink {
  animation: blink 1s infinite;
}

/* تغییر نشانگر موس برای مربع فعال */
.cursor-text {
  cursor: text;
}

/* بهبود افکت hover برای مربع‌های غیرفعال */
.w-12.h-12:not(.active-square):hover {
  transform: scale(1.1);
  transition: transform 0.2s ease-in-out;
}

/* اضافه کردن استایل برای مربع فعال اولیه */
.initial-active {
  background-color: rgba(99, 102, 241, 0.05);
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.2);
  animation: initialPulse 2s infinite;
}

@keyframes initialPulse {
  0% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(99, 102, 241, 0.4);
  }
  70% {
    transform: scale(1.05);
    box-shadow: 0 0 0 10px rgba(99, 102, 241, 0);
  }
  100% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(99, 102, 241, 0);
  }
}

/* انیمیشن برای آیکن‌ها */
@keyframes float {
  0% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-5px);
  }
  100% {
    transform: translateY(0px);
  }
}

.fas {
  animation: float 3s ease-in-out infinite;
}

/* انیمیشن برای لامپ راهنما */
@keyframes glow {
  0% {
    filter: drop-shadow(0 0 2px rgba(234, 179, 8, 0.5));
  }
  50% {
    filter: drop-shadow(0 0 8px rgba(234, 179, 8, 0.8));
  }
  100% {
    filter: drop-shadow(0 0 2px rgba(234, 179, 8, 0.5));
  }
}

.fa-lightbulb {
  animation: glow 2s ease-in-out infinite;
}

/* اضافه کردن استایل برای کیبورد */
.keyboard-key {
  min-width: 2.5rem;
  height: 3rem;
  font-size: 1.25rem;
  font-weight: 500;
  transition: all 0.2s;
}

.keyboard-key:active {
  transform: scale(0.95);
}

/* استایل برای سطر فعال و غیرفعال */
.active-row {
  background-color: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(8px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
}

.inactive-row {
  opacity: 0.7;
  filter: grayscale(20%);
}

/* تغییر استایل برای حالت غیرفعال کیبورد */
button:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}
</style> 