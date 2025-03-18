<template>
  <div class="flex flex-col items-center justify-center space-y-8">
    <!-- Score Display -->
    <div v-if="isDailyChallenge" class="flex items-center justify-between w-full max-w-2xl">
      <div class="flex items-center space-x-4 rtl:space-x-reverse">
        <div class="text-center">
          <div class="text-2xl font-bold text-gray-900 dark:text-white">{{ currentScore }}</div>
          <div class="text-sm text-gray-500 dark:text-gray-400">{{ t('score') }}</div>
        </div>
        <div class="text-center">
          <div class="text-2xl font-bold text-gray-900 dark:text-white">{{ guessCount }}/{{ maxAttempts }}</div>
          <div class="text-sm text-gray-500 dark:text-gray-400">{{ t('attempts') }}</div>
        </div>
      </div>
      <div class="text-center">
        <div class="text-2xl font-bold text-gray-900 dark:text-white">{{ personalBest }}</div>
        <div class="text-sm text-gray-500 dark:text-gray-400">{{ t('dailyChallengeBest') }}</div>
      </div>
    </div>

    <!-- Game Board -->
    <div class="grid grid-cols-5 gap-2">
      <div
        v-for="row in maxAttempts"
        :key="row"
        class="flex space-x-2 rtl:space-x-reverse"
      >
        <div
          v-for="col in 5"
          :key="col"
          class="w-14 h-14 border-2 rounded-lg flex items-center justify-center text-2xl font-bold uppercase"
          :class="[
            getCellColor(row - 1, col - 1),
            getCellAnimation(row - 1, col - 1)
          ]"
        >
          {{ getCellLetter(row - 1, col - 1) }}
        </div>
      </div>
    </div>

    <!-- Virtual Keyboard -->
    <div v-if="interactive" class="grid grid-cols-10 gap-1">
      <button
        v-for="key in keyboardLayout"
        :key="key"
        class="px-3 py-4 bg-gray-200 dark:bg-gray-700 rounded-lg text-lg font-bold uppercase hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
        :class="getKeyColor(key)"
        @click="handleKeyPress(key)"
      >
        {{ key }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'

const props = defineProps({
  word: {
    type: String,
    required: true
  },
  guesses: {
    type: Array,
    default: () => []
  },
  maxAttempts: {
    type: Number,
    default: 6
  },
  interactive: {
    type: Boolean,
    default: true
  },
  isDailyChallenge: {
    type: Boolean,
    default: false
  },
  currentScore: {
    type: Number,
    default: 0
  },
  guessCount: {
    type: Number,
    default: 0
  },
  personalBest: {
    type: Number,
    default: null
  }
})

const emit = defineEmits(['make-guess'])

const currentRow = computed(() => props.guesses.length)
const shake = ref(false)
const pressedKey = ref(null)

const keyboardLayout = [
  ['ض', 'ص', 'ث', 'ق', 'ف', 'غ', 'ع', 'ه', 'خ', 'ح', 'ج', 'چ'],
  ['ش', 'س', 'ی', 'ب', 'ل', 'ا', 'ت', 'ن', 'م', 'ک', 'گ'],
  ['Enter', 'ظ', 'ط', 'ز', 'ر', 'ذ', 'د', 'پ', 'و', 'Backspace']
]

const board = computed(() => {
  const rows = []
  // Fill with previous guesses
  for (let i = 0; i < props.guesses.length; i++) {
    rows.push(props.guesses[i].map(cell => ({
      letter: cell.letter,
      color: cell.color
    })))
  }
  // Fill remaining rows with empty cells
  for (let i = props.guesses.length; i < props.maxAttempts; i++) {
    rows.push(Array(props.word.length).fill({ letter: '', color: '' }))
  }
  return rows
})

const keyColors = computed(() => {
  const colors = {}
  props.guesses.forEach(guess => {
    guess.forEach(({ letter, color }) => {
      // Only update if the new color is "better"
      if (!colors[letter] || 
          (colors[letter] !== 'green' && (color === 'green' || colors[letter] !== 'yellow'))) {
        colors[letter] = color
      }
    })
  })
  return colors
})

function getKeyColor(key) {
  return keyColors.value[key] || ''
}

function handleKeyPress(key) {
  if (!props.interactive) return
  
  pressedKey.value = key
  setTimeout(() => pressedKey.value = null, 100)

  if (key === 'Backspace') {
    // Handle backspace
  } else if (key === 'Enter') {
    // Handle enter
    if (currentGuess.length === props.word.length) {
      makeGuess()
    } else {
      shakeBoard()
    }
  } else if (currentGuess.length < props.word.length) {
    // Add letter
    currentGuess.push(key)
  }
}

function shakeBoard() {
  shake.value = true
  setTimeout(() => shake.value = false, 500)
}

function makeGuess() {
  emit('make-guess', currentGuess.join(''))
  currentGuess = []
}

// Keyboard event handling
function handleKeyDown(event) {
  if (!props.interactive) return
  
  const key = event.key.toUpperCase()
  if (key === 'BACKSPACE' || key === 'ENTER' || keyboardLayout.flat().includes(key)) {
    handleKeyPress(key)
  }
}

onMounted(() => {
  window.addEventListener('keydown', handleKeyDown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyDown)
})

const getCellColor = (row, col) => {
  if (row >= props.guesses.length) {
    return 'border-gray-300 dark:border-gray-600'
  }

  const guess = props.guesses[row]
  if (!guess) return 'border-gray-300 dark:border-gray-600'

  const letter = guess[col]
  if (!letter) return 'border-gray-300 dark:border-gray-600'

  if (letter === props.word[col]) {
    return 'bg-green-500 border-green-500 text-white'
  }

  if (props.word.includes(letter)) {
    return 'bg-yellow-500 border-yellow-500 text-white'
  }

  return 'bg-gray-500 border-gray-500 text-white'
}

const getCellAnimation = (row, col) => {
  if (row === props.guesses.length - 1) {
    return 'animate-flip'
  }
  return ''
}

const getCellLetter = (row, col) => {
  if (row >= props.guesses.length) return ''
  return props.guesses[row][col] || ''
}
</script>

<style scoped>
.game-board {
  @apply flex flex-col gap-2 mx-auto max-w-lg;
}

.row {
  @apply flex gap-2 justify-center;
}

.cell {
  @apply w-14 h-14 border-2 border-gray-300 dark:border-gray-600 flex justify-center items-center text-2xl font-bold uppercase transition-colors duration-300;
}

.cell.green {
  @apply bg-green-500 dark:bg-green-600 border-green-500 dark:border-green-600 text-white;
}

.cell.yellow {
  @apply bg-yellow-500 dark:bg-yellow-600 border-yellow-500 dark:border-yellow-600 text-white;
}

.cell.gray {
  @apply bg-gray-500 dark:bg-gray-600 border-gray-500 dark:border-gray-600 text-white;
}

.keyboard {
  @apply flex flex-col gap-2 mx-auto max-w-2xl;
}

.keyboard-row {
  @apply flex gap-1 justify-center;
}

.key {
  @apply min-w-[2.5rem] h-14 px-2 rounded bg-gray-200 dark:bg-gray-700 text-gray-800 dark:text-gray-200 
         font-medium transition-all duration-200 hover:bg-gray-300 dark:hover:bg-gray-600 
         disabled:opacity-50 disabled:cursor-not-allowed;
}

.key.green {
  @apply bg-green-500 dark:bg-green-600 text-white hover:bg-green-600 dark:hover:bg-green-700;
}

.key.yellow {
  @apply bg-yellow-500 dark:bg-yellow-600 text-white hover:bg-yellow-600 dark:hover:bg-yellow-700;
}

.key.gray {
  @apply bg-gray-500 dark:bg-gray-600 text-white hover:bg-gray-600 dark:hover:bg-gray-700;
}

.key-press {
  @apply transform scale-90;
}

/* Animations */
.pop-in {
  animation: pop-in 0.1s ease-in-out;
}

.flip-in {
  animation: flip-in 0.3s ease-in-out forwards;
  animation-delay: var(--flip-delay);
}

.shake {
  animation: shake 0.5s ease-in-out;
}

@keyframes pop-in {
  0% {
    transform: scale(0.8);
    opacity: 0;
  }
  100% {
    transform: scale(1);
    opacity: 1;
  }
}

@keyframes flip-in {
  0% {
    transform: rotateX(0);
  }
  50% {
    transform: rotateX(90deg);
  }
  100% {
    transform: rotateX(0);
  }
}

@keyframes shake {
  0%, 100% {
    transform: translateX(0);
  }
  25% {
    transform: translateX(-5px);
  }
  75% {
    transform: translateX(5px);
  }
}
</style> 