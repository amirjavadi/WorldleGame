<template>
  <div class="space-y-6">
    <!-- Score Display for Daily Challenge -->
    <div v-if="showScore" class="flex justify-between items-center bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/20 dark:to-purple-900/20 p-4 rounded-lg">
      <div class="flex items-center space-x-4 rtl:space-x-reverse">
        <div class="text-center">
          <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ score }}</div>
          <div class="text-sm text-indigo-500 dark:text-indigo-500">امتیاز</div>
        </div>
        <div class="text-center">
          <div class="text-2xl font-bold text-purple-600 dark:text-purple-400">{{ guessCount }}/{{ maxAttempts }}</div>
          <div class="text-sm text-purple-500 dark:text-purple-500">حدس‌ها</div>
        </div>
      </div>
      <div v-if="personalBest" class="text-center bg-yellow-100 dark:bg-yellow-900/20 px-4 py-2 rounded-lg">
        <div class="text-sm font-medium text-yellow-800 dark:text-yellow-300">رکورد شخصی</div>
        <div class="text-lg font-bold text-yellow-700 dark:text-yellow-400">{{ personalBest }}</div>
      </div>
    </div>

    <!-- Game Board -->
    <div class="game-board">
      <div v-for="(row, rowIndex) in board" :key="rowIndex" class="row">
        <div
          v-for="(cell, cellIndex) in row"
          :key="cellIndex"
          :class="[
            'cell',
            cell.color,
            { 'pop-in': cell.letter && !cell.color },
            { 'flip-in': cell.color },
            { 'shake': rowIndex === currentRow && shake }
          ]"
          :style="{ 
            animationDelay: cell.color ? `${cellIndex * 0.1}s` : '0s',
            '--flip-delay': `${cellIndex * 0.1}s`
          }"
        >
          {{ cell.letter }}
        </div>
      </div>
    </div>

    <!-- Virtual Keyboard -->
    <div class="keyboard">
      <div v-for="(row, rowIndex) in keyboardLayout" :key="rowIndex" class="keyboard-row">
        <button
          v-for="key in row"
          :key="key"
          :class="[
            'key',
            getKeyColor(key),
            { 'key-press': pressedKey === key }
          ]"
          @click="handleKeyPress(key)"
          :disabled="!isInteractive"
        >
          <template v-if="key === 'Backspace'">
            <i class="fas fa-backspace"></i>
          </template>
          <template v-else-if="key === 'Enter'">
            <i class="fas fa-level-down-alt fa-rotate-90"></i>
          </template>
          <template v-else>
            {{ key }}
          </template>
        </button>
      </div>
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
  showScore: {
    type: Boolean,
    default: false
  },
  score: {
    type: Number,
    default: 0
  },
  personalBest: {
    type: Number,
    default: null
  },
  isInteractive: {
    type: Boolean,
    default: true
  }
})

const emit = defineEmits(['make-guess'])

const currentRow = computed(() => props.guesses.length)
const guessCount = computed(() => props.guesses.length)
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
  if (!props.isInteractive) return
  
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
  if (!props.isInteractive) return
  
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