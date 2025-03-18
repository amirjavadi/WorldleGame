<template>
  <div class="min-h-screen relative overflow-hidden">
    <!-- Animated background -->
    <div class="fixed inset-0 bg-gradient-to-br from-blue-50/60 via-indigo-50/60 to-purple-50/60 dark:from-slate-900/90 dark:via-slate-900/90 dark:to-slate-900/90">
      <!-- Animated circles -->
      <div class="absolute top-0 left-0 w-full h-full">
        <div class="absolute w-[500px] h-[500px] bg-gradient-to-r from-rose-100/10 to-indigo-100/10 dark:from-rose-900/5 dark:to-indigo-900/5 rounded-full blur-3xl animate-float-slow top-[-200px] left-[-200px]"></div>
        <div class="absolute w-[400px] h-[400px] bg-gradient-to-r from-blue-100/10 to-violet-100/10 dark:from-blue-900/5 dark:to-violet-900/5 rounded-full blur-3xl animate-float-medium top-[40%] right-[-100px]"></div>
        <div class="absolute w-[600px] h-[600px] bg-gradient-to-r from-indigo-100/10 to-purple-100/10 dark:from-indigo-900/5 dark:to-purple-900/5 rounded-full blur-3xl animate-float-fast bottom-[-200px] left-[30%]"></div>
      </div>
      <!-- Subtle grid pattern -->
      <div class="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNDAiIGhlaWdodD0iNDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGRlZnM+PHBhdHRlcm4gaWQ9ImdyaWQiIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgcGF0dGVyblVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+PHBhdGggZD0iTSAwIDEwIEwgNDAgMTAgTSAxMCAwIEwgMTAgNDBNIDAgMjAgTCA0MCAyMCBNIDIwIDAgTCAyMCA0MCBNIDAgMzAgTCA0MCAzMCBNIDMwIDAgTCAzMCA0MCIgZmlsbD0ibm9uZSIgc3Ryb2tlPSJjdXJyZW50Q29sb3IiIHN0cm9rZS13aWR0aD0iMSIgc3Ryb2tlLW9wYWNpdHk9IjAuMDIiLz48L3BhdHRlcm4+PC9kZWZzPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9InVybCgjZ3JpZCkiLz48L3N2Zz4=')] opacity-30"></div>
    </div>

    <!-- Main content -->
    <div class="relative z-10">
      <MainNav />
      <main class="flex-1 container mx-auto px-4 py-4">
        <div class="px-4 py-2 sm:px-0">
          <div class="flex flex-col items-center space-y-6">
            <div class="w-full max-w-md">
              <!-- Daily Challenge Section -->
              <div v-if="isAuthenticated" 
                   class="flex items-center justify-between mb-4 bg-gradient-to-r from-indigo-50/80 via-purple-50/80 to-pink-50/80 dark:from-indigo-900/20 dark:via-purple-900/20 dark:to-pink-900/20 py-2 px-4 rounded-xl shadow-md transform hover:scale-[1.01] transition-all duration-300 hover:shadow-lg border border-indigo-100/50 dark:border-indigo-800/20 backdrop-blur-sm">
                <div class="flex items-center space-x-3 rtl:space-x-reverse">
                  <div class="relative">
                    <button
                      @click="showDailyModal = true"
                      class="group relative flex items-center space-x-2 rtl:space-x-reverse px-4 py-1.5 bg-gradient-to-r from-indigo-200/90 via-purple-200/90 to-pink-200/90 dark:from-indigo-800/40 dark:via-purple-800/40 dark:to-pink-800/40 text-indigo-700 dark:text-indigo-200 rounded-lg shadow-sm hover:shadow-md transition-all duration-500 transform hover:scale-[1.02] backdrop-blur-sm overflow-hidden"
                    >
                      <i class="fas fa-calendar-day text-lg relative z-10 group-hover:rotate-12 transition-transform duration-300"></i>
                      <span class="font-bold relative z-10 group-hover:tracking-wider transition-all duration-300">{{ t('dailyChallenge') }}</span>
                      <!-- Animated background gradient -->
                      <div class="absolute inset-0 bg-gradient-to-r from-indigo-200/50 via-purple-200/50 to-pink-200/50 dark:from-indigo-700/30 dark:via-purple-700/30 dark:to-pink-700/30 opacity-0 group-hover:opacity-100 transition-opacity duration-500"></div>
                      <!-- Shine effect -->
                      <div class="absolute inset-0 transform translate-x-full group-hover:translate-x-[-100%] transition-transform duration-1000 bg-gradient-to-r from-transparent via-white/20 to-transparent"></div>
                    </button>
                    <div class="absolute -top-1 -right-1 w-1.5 h-1.5 bg-yellow-300/80 rounded-full animate-ping"></div>
                    <div class="absolute -bottom-1 -left-1 w-1.5 h-1.5 bg-purple-300/80 rounded-full animate-ping delay-150"></div>
                  </div>
                  <div class="relative">
                    <i class="fas fa-trophy text-xl text-yellow-400/90 transform transition-transform duration-200 hover:scale-110 hover:text-yellow-500/90"></i>
                    <div class="absolute -top-1 -right-1 w-1.5 h-1.5 bg-yellow-300/80 rounded-full animate-ping"></div>
                  </div>
                </div>
                
                <div class="flex items-center space-x-2 rtl:space-x-reverse bg-white/30 dark:bg-gray-800/30 rounded-lg px-3 py-1 shadow-inner">
                  <div class="text-center">
                    <div class="text-sm font-bold text-transparent bg-clip-text bg-gradient-to-r from-indigo-500/90 to-purple-500/90 dark:from-indigo-400/90 dark:to-purple-400/90">{{ hours }}</div>
                    <div class="text-xs text-indigo-400/70 dark:text-indigo-400/70">{{ t('hours') }}</div>
                  </div>
                  <div class="text-sm font-bold text-indigo-400/70 dark:text-indigo-400/70 animate-pulse">:</div>
                  <div class="text-center">
                    <div class="text-sm font-bold text-transparent bg-clip-text bg-gradient-to-r from-indigo-500/90 to-purple-500/90 dark:from-indigo-400/90 dark:to-purple-400/90">{{ minutes }}</div>
                    <div class="text-xs text-indigo-400/70 dark:text-indigo-400/70">{{ t('minutes') }}</div>
                  </div>
                  <div class="text-sm font-bold text-indigo-400/70 dark:text-indigo-400/70 animate-pulse">:</div>
                  <div class="text-center">
                    <div class="text-sm font-bold text-transparent bg-clip-text bg-gradient-to-r from-indigo-500/90 to-purple-500/90 dark:from-indigo-400/90 dark:to-purple-400/90">{{ seconds }}</div>
                    <div class="text-xs text-indigo-400/70 dark:text-indigo-400/70">{{ t('seconds') }}</div>
                  </div>
                </div>
              </div>

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
                        currentRow === row - 1 && currentCol === col - 1 ? 'active-square' : '',
                        currentRow === row - 1 && col - 1 === currentCol ? 'waiting-input' : '',
                        completedRows.value && completedRows.value.has(row - 1) ? 'flip-animation' : '',
                        currentRow === row - 1 ? 'active-row' : 'inactive-row'
                      ]"
                    >
                      {{ guesses[row - 1]?.[col - 1] || '' }}
                      <div v-if="currentRow === row - 1 && currentCol === col - 1" 
                           class="absolute bottom-2 left-1/2 transform -translate-x-1/2 cursor-blink">
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
        <Transition>
          <div v-if="showStats" 
               class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[60]">
            <StatsOverview
              :games-played="gamesPlayed"
              :win-rate="winRate"
              :current-streak="currentStreak"
              :best-streak="bestStreak"
              :show-play-again="gameOver"
              @close="closeStats"
              @play-again="handlePlayAgain"
            />
          </div>
        </Transition>
      </Teleport>

      <!-- انیمیشن‌های برد و باخت -->
      <Teleport to="body">
        <Transition>
          <WinAnimation v-if="gameOver && gameWon" 
                       :score="score"
                       :current-streak="currentStreak"
                       @playAgain="handlePlayAgain" />
        </Transition>
        <Transition>
          <LoseAnimation v-if="gameOver && !gameWon" 
                        :correct-word="correctWord"
                        :games-played="gamesPlayed"
                        :win-rate="winRate"
                        @playAgain="handlePlayAgain" />
        </Transition>
      </Teleport>

      <!-- کیبورد -->
      <div class="fixed bottom-0 left-0 right-0">
        <!-- نوار بالای کیبورد -->
        <div 
          @click="showKeyboard = !showKeyboard"
          class="w-full bg-white/95 dark:bg-gray-800/95 backdrop-blur-sm cursor-pointer flex justify-center items-center py-1 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors duration-200 border-t border-gray-200 dark:border-gray-700"
        >
          <div class="w-16 h-1 bg-gray-300 dark:bg-gray-600 rounded-full"></div>
        </div>
        <!-- کیبورد -->
        <div 
          v-show="showKeyboard"
          class="bg-white/95 dark:bg-gray-800/95 backdrop-blur-sm shadow-lg p-4 transition-all duration-300 transform"
          :class="showKeyboard ? 'translate-y-0' : 'translate-y-full'"
        >
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
              <div class="grid grid-cols-11 gap-1">
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
                  @click="handleKeyPress({ key: 'Enter' })"
                  class="col-span-2 p-2 text-center rounded-lg bg-green-100 dark:bg-green-900 hover:bg-green-200 dark:hover:bg-green-800 transition-all duration-200"
                  :disabled="gameOver"
                >
                  {{ t('enter') }}
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
              <div class="grid grid-cols-11 gap-1">
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
                  @click="handleKeyPress({ key: 'Enter' })"
                  class="col-span-2 p-2 text-center rounded-lg bg-green-100 dark:bg-green-900 hover:bg-green-200 dark:hover:bg-green-800 transition-all duration-200"
                  :disabled="gameOver"
                >
                  {{ t('enter') }}
                </button>
                <button
                  @click="handleKeyPress({ key: 'Backspace' })"
                  class="col-span-2 p-2 text-center rounded-lg bg-red-100 dark:bg-red-900 hover:bg-red-200 dark:hover:bg-red-800 transition-all duration-200"
                  :disabled="gameOver"
                >
                  <i class="fas fa-backspace"></i>
                </button>
              </div>
            </template>
          </div>
        </div>
      </div>

      <!-- مدال مهمان -->
      <Teleport to="body">
        <Transition>
          <GuestModal
            v-if="showGuestModal"
            @close="showGuestModal = false"
          />
        </Transition>
      </Teleport>

      <DailyModal
        v-if="showDailyModal"
        @close="showDailyModal = false"
        @login="handleLoginClick"
        @register="handleRegisterClick"
      />

      <ProfileModal
        v-if="showProfileModal"
        @close="showProfileModal = false"
      />
    </div>
  </div>
</template>

<script setup>
import { useTranslations } from '../composables/useTranslations'
import { useSounds } from '../composables/useSounds'
import { useRouter } from 'vue-router'
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { useColorMode } from '#imports'
import WinAnimation from '~/components/WinAnimation.vue'
import LoseAnimation from '~/components/LoseAnimation.vue'
import MainNav from '~/components/Navigation/MainNav.vue'
import StatsOverview from '~/components/Stats/Overview.vue'
import GuestModal from '~/components/GuestModal.vue'
import DailyModal from '~/components/Daily/DailyModal.vue'
import ProfileModal from '~/components/Profile/ProfileModal.vue'
import { navigateTo } from '#app'
import { useAuthStore } from '~/stores/auth'
import { useDailyChallengeStore } from '~/stores/dailyChallenge'

const { t, locale, setLocale } = useTranslations()
const { playSound, isSoundEnabled, toggleSound } = useSounds()
const router = useRouter()
const colorMode = useColorMode()
const theme = computed(() => colorMode.value)
const dailyStore = useDailyChallengeStore()
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
const revealedHelpLetters = ref(new Set()) // برای ذخیره حروفی که قبلاً به عنوان کمکی نشان داده شده‌اند
const showKeyboard = ref(true) // برای نمایش/مخفی کردن کیبورد
const showGuestModal = ref(false)
const showDailyModal = ref(false)
const showProfileModal = ref(false)
const authStore = useAuthStore()
const isAuthenticated = computed(() => authStore.isLoggedIn && !authStore.isGuest)
const timeUntilNext = ref(0)

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
  const newTheme = colorMode.value === 'light' ? 'dark' : 'light'
  colorMode.preference = newTheme
  if (process.client) {
    localStorage.setItem('preferredTheme', newTheme)
  }
}

const getLetterClass = (letter, index, row) => {
  if (!letter) return 'border-gray-300 dark:border-gray-600'
  
  // اگر این حرف کمکی است، همیشه سبز نشان داده شود
  if (helpLetters.value.has(`${row-1}-${index}`)) {
    return theme.value === 'light' ? 'bg-green-200 text-green-800 border-green-300' : 'bg-green-500 text-white border-green-500'
  }
  
  // اگر ردیف هنوز کامل نشده، فقط حاشیه خاکستری نشان بده
  if (!completedRows.value || !completedRows.value.has(row - 1)) {
    return 'border-gray-300 dark:border-gray-600'
  }
  
  // برای ردیف‌های کامل شده، رنگ‌ها را نشان بده
  if (letter === correctWord.value[index]) {
    return theme.value === 'light' ? 'bg-green-200 text-green-800 border-green-300' : 'bg-green-500 text-white border-green-500'
  }

  // بررسی تعداد تکرار حرف در کلمه صحیح
  const letterCount = correctWord.value.split('').filter(l => l === letter).length
  
  // بررسی تعداد حروف سبز (حروف درست در جای درست) برای این حرف
  const greenCount = guesses.value[row - 1].reduce((count, l, i) => {
    return count + (l === letter && l === correctWord.value[i] ? 1 : 0)
  }, 0)
  
  // بررسی تعداد حروف زرد قبل از این ایندکس
  const yellowCount = guesses.value[row - 1].slice(0, index).reduce((count, l, i) => {
    return count + (l === letter && l !== correctWord.value[i] && correctWord.value.includes(l) ? 1 : 0)
  }, 0)
  
  // اگر هنوز جا برای نمایش زرد داریم
  if (correctWord.value.includes(letter) && yellowCount + greenCount < letterCount) {
    return theme.value === 'light' ? 'bg-yellow-200 text-yellow-800 border-yellow-300' : 'bg-yellow-500 text-white border-yellow-500'
  }
  
  return theme.value === 'light' ? 'bg-gray-200 text-gray-800 border-gray-300' : 'bg-gray-500 text-white border-gray-500'
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

  // پیدا کردن موقعیت‌های خالی در سطر فعلی و حروف متناظر آنها که قبلاً نشان داده نشده‌اند
  const availablePositions = []
  for (let i = 0; i < wordLength.value; i++) {
    const letter = correctWord.value[i]
    if (!guesses.value[currentRow.value]?.[i] && 
        !helpLetters.value.has(`${currentRow.value}-${i}`) && 
        !revealedHelpLetters.value.has(letter)) {
      availablePositions.push({ index: i, letter })
    }
  }

  if (availablePositions.length === 0) {
    alert(t('noNewHelpAvailable'))
    return
  }

  // انتخاب یک موقعیت تصادفی از موقعیت‌های موجود
  const randomPosition = availablePositions[Math.floor(Math.random() * availablePositions.length)]

  // قرار دادن حرف درست در موقعیت انتخاب شده
  if (!guesses.value[currentRow.value]) {
    guesses.value[currentRow.value] = []
  }
  guesses.value[currentRow.value][randomPosition.index] = randomPosition.letter
  helpLetters.value.add(`${currentRow.value}-${randomPosition.index}`)
  revealedHelpLetters.value.add(randomPosition.letter)

  // انتقال فوکوس به اولین مربع خالی
  const nextEmpty = findNextEmptyPosition()
  currentCol.value = nextEmpty !== -1 ? nextEmpty : randomPosition.index + 1

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
  
  if (key === 'backspace' || key === 'delete') {
    if (currentCol.value > 0) {
      playSound('backspace')
      currentCol.value--
      guesses.value[currentRow.value][currentCol.value] = ''
    }
    return
  }

  if (key === 'enter') {
    if (currentCol.value === wordLength.value) {
      // اگر حدس درست باشد
      if (guesses.value[currentRow.value].join('') === correctWord.value) {
        playSound('win')
        gameWon.value = true
        gameOver.value = true
        completedRows.value.add(currentRow.value)
        updateStats(true)
      } else if (currentRow.value === maxGuesses.value - 1) {
        // اگر بازی تمام شده باشد
        playSound('gameOver')
        gameOver.value = true
        gameWon.value = false
        completedRows.value.add(currentRow.value)
        updateStats(false)
      } else {
        // حدس اشتباه
        playSound('error')
        currentRow.value++
        currentCol.value = 0
        completedRows.value.add(currentRow.value - 1)
        updateDisabledLetters()
      }
    }
    return
  }

  // برای حروف عادی
  if (currentCol.value < wordLength.value && isValidKey(key)) {
    playSound('type')
    if (!guesses.value[currentRow.value]) {
      guesses.value[currentRow.value] = []
    }
    guesses.value[currentRow.value][currentCol.value] = key
    currentCol.value++
  }
}

const isValidKey = (key) => {
  if (!key) return false
  const validLetters = locale.value === 'fa' 
    ? 'ضصثقفغعهخحجشسیبلاتنمکظطزرذدپو'
    : 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'
  return validLetters.includes(key)
}

const updateStats = (won) => {
  gamesPlayed.value++
  if (won) {
    currentStreak.value++
    bestStreak.value = Math.max(bestStreak.value, currentStreak.value)
    score.value += calculateScore(currentRow.value + 1)
    helpCount.value++
  } else {
    currentStreak.value = 0
  }
  winRate.value = Math.round((currentStreak.value / gamesPlayed.value) * 100)
}

const handlePlayAgain = () => {
  // حذف نمایش آمار
  resetGame()
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
  helpLetters.value.clear()
  revealedHelpLetters.value.clear()
  showStats.value = false
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

let timer
onMounted(() => {
  // تنظیمات اولیه
  document.documentElement.dir = locale.value === 'fa' ? 'rtl' : 'ltr'
  document.documentElement.lang = locale.value
  document.body.style.textAlign = locale.value === 'fa' ? 'right' : 'left'
  
  // بازیابی تم از localStorage
  if (process.client) {
    const savedTheme = localStorage.getItem('preferredTheme')
    if (savedTheme) {
      colorMode.preference = savedTheme
    }
    
    // نمایش مدال مهمان اگر کاربر لاگین نکرده باشد
    if (!localStorage.getItem('token') && !localStorage.getItem('guestModalShown')) {
      showGuestModal.value = true
      localStorage.setItem('guestModalShown', 'true')
    }
  }
  
  window.addEventListener('keydown', handleKeyPress)
  
  // تنظیم تایمر چالش روزانه
  if (isAuthenticated.value) {
    dailyStore.fetchTodaysChallenge()
    updateTimer()
    timer = setInterval(updateTimer, 1000)
  }
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyPress)
  if (timer) clearInterval(timer)
})

// اضافه کردن تابع جدید برای کلاس‌های کیبورد
const getKeyboardLetterClass = (letter) => {
  let classes = 'text-gray-900 dark:text-white'
  
  if (isLetterDisabled(letter)) {
    classes = 'text-[#646971] dark:text-gray-400 bg-[#464b514d] dark:bg-gray-600'
  } else if (guesses.value[currentRow.value]?.includes(letter)) {
    // فقط حروفی که در حدس فعلی استفاده شده‌اند را هایلایت کن
    classes += ' bg-gray-200 dark:bg-gray-500'
  }
  
  return classes
}

const handleLoginSuccess = () => {
  // Handle login success
}

const handleRegisterSuccess = () => {
  // Handle register success
}

const logout = () => {
  // Handle logout
}

const handleLoginClick = () => {
  navigateTo('/login')
}

const handleRegisterClick = () => {
  navigateTo('/register')
}

const updateTimer = () => {
  const now = new Date()
  const tomorrow = new Date(now)
  tomorrow.setDate(tomorrow.getDate() + 1)
  tomorrow.setHours(0, 0, 0, 0)
  timeUntilNext.value = tomorrow - now
}

const hours = computed(() => Math.floor(timeUntilNext.value / (1000 * 60 * 60)).toString().padStart(2, '0'))
const minutes = computed(() => Math.floor((timeUntilNext.value % (1000 * 60 * 60)) / (1000 * 60)).toString().padStart(2, '0'))
const seconds = computed(() => Math.floor((timeUntilNext.value % 1000) / 100).toString().padStart(2, '0'))
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

/* استایل برای مربع فعال */
.active-square {
  animation: activeSquarePulse 2s infinite;
  border-width: 2px !important;
  border-color: #6366f1 !important;
  background-color: rgba(99, 102, 241, 0.05) !important;
  box-shadow: 0 0 8px rgba(99, 102, 241, 0.2);
}

/* استایل برای مربع منتظر ورودی */
.waiting-input {
  border-width: 2px !important;
  border-color: #818cf8 !important;
  background-color: rgba(129, 140, 248, 0.05) !important;
  box-shadow: 0 0 4px rgba(129, 140, 248, 0.15);
}

@keyframes activeSquarePulse {
  0%, 100% {
    box-shadow: 0 0 4px rgba(99, 102, 241, 0.2);
  }
  50% {
    box-shadow: 0 0 8px rgba(99, 102, 241, 0.3);
  }
}

/* نشانگر چشمک‌زن */
.cursor-blink {
  animation: blink 1s infinite;
  width: 2px !important;
  height: 2px !important;
  background-color: #6366f1 !important;
  border-radius: 50%;
}

@keyframes blink {
  0%, 100% {
    opacity: 1;
    transform: scale(1);
  }
  50% {
    opacity: 0.5;
    transform: scale(0.8);
  }
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

/* انیمیشن‌های ظاهر و ناپدید شدن */
.v-enter-active,
.v-leave-active {
  transition: opacity 0.5s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}

/* انیمیشن‌های جدید برای بک‌گراند */
@keyframes float-slow {
  0%, 100% {
    transform: translate(0, 0) rotate(0deg);
  }
  25% {
    transform: translate(20px, 20px) rotate(2deg);
  }
  50% {
    transform: translate(-10px, 30px) rotate(-1deg);
  }
  75% {
    transform: translate(-20px, 10px) rotate(1deg);
  }
}

@keyframes float-medium {
  0%, 100% {
    transform: translate(0, 0) rotate(0deg);
  }
  33% {
    transform: translate(-30px, -20px) rotate(-2deg);
  }
  66% {
    transform: translate(20px, -10px) rotate(2deg);
  }
}

@keyframes float-fast {
  0%, 100% {
    transform: translate(0, 0) rotate(0deg);
  }
  50% {
    transform: translate(15px, -15px) rotate(1deg);
  }
}

.animate-float-slow {
  animation: float-slow 20s ease-in-out infinite;
}

.animate-float-medium {
  animation: float-medium 15s ease-in-out infinite;
}

.animate-float-fast {
  animation: float-fast 10s ease-in-out infinite;
}

/* اضافه کردن transition برای تغییر تم */
.min-h-screen {
  transition: background-color 0.5s ease-in-out;
}

/* بهبود کیفیت blur */
.blur-3xl {
  backdrop-filter: blur(64px);
  -webkit-backdrop-filter: blur(64px);
}
</style> 