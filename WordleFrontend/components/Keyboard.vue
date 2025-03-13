<template>
  <div class="keyboard">
    <div v-for="(row, rowIndex) in keyboardLayout" :key="rowIndex" class="keyboard-row">
      <button
        v-for="key in row"
        :key="key"
        :class="['key', getKeyColor(key)]"
        @click="handleKeyPress(key)"
      >
        {{ key }}
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Keyboard',
  data() {
    return {
      keyboardLayout: [
        ['Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'],
        ['A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L'],
        ['Enter', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', 'Backspace']
      ]
    }
  },
  props: {
    keyColors: {
      type: Object,
      default: () => ({})
    }
  },
  methods: {
    handleKeyPress(key) {
      this.$emit('keypress', key)
    },
    getKeyColor(key) {
      return this.keyColors[key] || ''
    }
  }
}
</script>

<style scoped>
.keyboard {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin: 20px auto;
  max-width: 500px;
}

.keyboard-row {
  display: flex;
  gap: 6px;
  justify-content: center;
}

.key {
  min-width: 40px;
  height: 58px;
  border: none;
  border-radius: 4px;
  background-color: #d3d6da;
  color: #1a1a1b;
  font-size: 1.25rem;
  font-weight: bold;
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0 10px;
  transition: background-color 0.2s;
}

.key:hover {
  background-color: #c4c7ca;
}

.key.green {
  background-color: #6aaa64;
  color: white;
}

.key.yellow {
  background-color: #c9b458;
  color: white;
}

.key.gray {
  background-color: #787c7e;
  color: white;
}

.key[data-key="Enter"],
.key[data-key="Backspace"] {
  min-width: 65px;
  font-size: 1rem;
}
</style> 