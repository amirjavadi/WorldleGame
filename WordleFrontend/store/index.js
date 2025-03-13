export const state = () => ({
  auth: {
    isAuthenticated: false,
    user: null,
    token: null
  },
  game: {
    board: Array(6).fill().map(() => Array(5).fill({ letter: '', color: '' })),
    currentRow: 0,
    currentCol: 0,
    gameOver: false,
    isWon: false,
    keyColors: {},
    dailyWord: null
  }
})

export const mutations = {
  SET_AUTH(state, { user, token }) {
    state.auth.isAuthenticated = true
    state.auth.user = user
    state.auth.token = token
  },
  CLEAR_AUTH(state) {
    state.auth.isAuthenticated = false
    state.auth.user = null
    state.auth.token = null
  },
  UPDATE_BOARD(state, { row, col, letter, color }) {
    state.game.board[row][col] = { letter, color }
  },
  SET_CURRENT_POSITION(state, { row, col }) {
    state.game.currentRow = row
    state.game.currentCol = col
  },
  SET_GAME_OVER(state, isWon) {
    state.game.gameOver = true
    state.game.isWon = isWon
  },
  UPDATE_KEY_COLORS(state, colors) {
    state.game.keyColors = { ...state.game.keyColors, ...colors }
  },
  SET_DAILY_WORD(state, word) {
    state.game.dailyWord = word
  },
  RESET_GAME(state) {
    state.game.board = Array(6).fill().map(() => Array(5).fill({ letter: '', color: '' }))
    state.game.currentRow = 0
    state.game.currentCol = 0
    state.game.gameOver = false
    state.game.isWon = false
    state.game.keyColors = {}
  }
}

export const actions = {
  async login({ commit }, credentials) {
    try {
      const { data } = await this.$axios.post('/auth/login', credentials)
      commit('SET_AUTH', {
        user: data.user,
        token: data.token
      })
      localStorage.setItem('token', data.token)
      return data
    } catch (error) {
      throw error
    }
  },
  async register({ commit }, userData) {
    try {
      const { data } = await this.$axios.post('/auth/register', userData)
      commit('SET_AUTH', {
        user: data.user,
        token: data.token
      })
      localStorage.setItem('token', data.token)
      return data
    } catch (error) {
      throw error
    }
  },
  async logout({ commit }) {
    commit('CLEAR_AUTH')
    localStorage.removeItem('token')
  },
  async submitGuess({ commit, state }, guess) {
    try {
      const { data } = await this.$axios.post('/game/guess', {
        guess,
        userId: state.auth.user.id
      })
      return data
    } catch (error) {
      throw error
    }
  },
  async getDailyWord({ commit }) {
    try {
      const { data } = await this.$axios.get('/game/daily-word')
      commit('SET_DAILY_WORD', data.word)
      return data.word
    } catch (error) {
      throw error
    }
  }
} 