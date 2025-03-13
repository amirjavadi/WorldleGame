<template>
  <div class="game-page">
    <h1 class="text-center mb-4">Wordle Game</h1>
    
    <game-board :board="board" />
    
    <keyboard
      :key-colors="keyColors"
      @keypress="handleKeyPress"
    />

    <v-dialog v-model="showResultModal" max-width="400">
      <v-card>
        <v-card-title class="text-h5">
          {{ isWon ? 'Congratulations!' : 'Game Over!' }}
        </v-card-title>
        <v-card-text>
          <p v-if="isWon">
            You won in {{ currentRow + 1 }} guesses!
          </p>
          <p v-else>
            The word was: <strong>{{ dailyWord }}</strong>
          </p>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="shareResult">
            Share
          </v-btn>
          <v-btn color="primary" @click="resetGame">
            Play Again
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import GameBoard from '~/components/GameBoard.vue'
import Keyboard from '~/components/Keyboard.vue'

export default {
  name: 'GamePage',
  components: {
    GameBoard,
    Keyboard
  },
  data() {
    return {
      showResultModal: false,
      currentGuess: ''
    }
  },
  computed: {
    board() {
      return this.$store.state.game.board
    },
    currentRow() {
      return this.$store.state.game.currentRow
    },
    currentCol() {
      return this.$store.state.game.currentCol
    },
    gameOver() {
      return this.$store.state.game.gameOver
    },
    isWon() {
      return this.$store.state.game.isWon
    },
    keyColors() {
      return this.$store.state.game.keyColors
    },
    dailyWord() {
      return this.$store.state.game.dailyWord
    }
  },
  watch: {
    gameOver(newValue) {
      if (newValue) {
        this.showResultModal = true
      }
    }
  },
  async created() {
    await this.$store.dispatch('getDailyWord')
  },
  methods: {
    async handleKeyPress(key) {
      if (this.gameOver) return

      if (key === 'Enter') {
        if (this.currentGuess.length === 5) {
          await this.submitGuess()
        }
      } else if (key === 'Backspace') {
        if (this.currentCol > 0) {
          this.$store.commit('UPDATE_BOARD', {
            row: this.currentRow,
            col: this.currentCol - 1,
            letter: '',
            color: ''
          })
          this.$store.commit('SET_CURRENT_POSITION', {
            row: this.currentRow,
            col: this.currentCol - 1
          })
          this.currentGuess = this.currentGuess.slice(0, -1)
        }
      } else if (this.currentCol < 5) {
        this.$store.commit('UPDATE_BOARD', {
          row: this.currentRow,
          col: this.currentCol,
          letter: key,
          color: ''
        })
        this.$store.commit('SET_CURRENT_POSITION', {
          row: this.currentRow,
          col: this.currentCol + 1
        })
        this.currentGuess += key
      }
    },
    async submitGuess() {
      try {
        const result = await this.$store.dispatch('submitGuess', this.currentGuess)
        this.updateBoardWithResult(result)
        this.currentGuess = ''
      } catch (error) {
        console.error('Error submitting guess:', error)
      }
    },
    updateBoardWithResult(result) {
      const { feedback, colors } = result
      feedback.forEach((color, index) => {
        this.$store.commit('UPDATE_BOARD', {
          row: this.currentRow,
          col: index,
          letter: this.currentGuess[index],
          color
        })
      })
      this.$store.commit('UPDATE_KEY_COLORS', colors)
    },
    shareResult() {
      const emoji = this.isWon ? '🟩' : '🟥'
      const text = `Wordle ${this.currentRow + 1}/6\n\n${this.getShareText()}\n\n${emoji}`
      navigator.clipboard.writeText(text)
      this.$toast.success('Result copied to clipboard!')
    },
    getShareText() {
      return this.board
        .slice(0, this.currentRow + 1)
        .map(row => row.map(cell => {
          switch (cell.color) {
            case 'green': return '🟩'
            case 'yellow': return '🟨'
            case 'gray': return '⬜'
            default: return '⬜'
          }
        }).join(''))
        .join('\n')
    },
    resetGame() {
      this.$store.commit('RESET_GAME')
      this.showResultModal = false
      this.currentGuess = ''
    }
  }
}
</script>

<style scoped>
.game-page {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.text-center {
  text-align: center;
}

.mb-4 {
  margin-bottom: 1rem;
}
</style> 