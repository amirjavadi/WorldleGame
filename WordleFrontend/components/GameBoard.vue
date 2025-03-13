<template>
  <div class="game-board">
    <div v-for="(row, rowIndex) in board" :key="rowIndex" class="row">
      <div
        v-for="(cell, cellIndex) in row"
        :key="cellIndex"
        :class="['cell', cell.color]"
        :style="{ animationDelay: cellIndex * 0.1 + 's' }"
      >
        {{ cell.letter }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'GameBoard',
  props: {
    board: {
      type: Array,
      required: true
    }
  }
}
</script>

<style scoped>
.game-board {
  display: flex;
  flex-direction: column;
  gap: 5px;
  margin: 20px auto;
  max-width: 500px;
}

.row {
  display: flex;
  gap: 5px;
  justify-content: center;
}

.cell {
  width: 60px;
  height: 60px;
  border: 2px solid #d3d6da;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 2rem;
  font-weight: bold;
  text-transform: uppercase;
  animation: flip 0.5s ease forwards;
}

.cell.green {
  background-color: #6aaa64;
  border-color: #6aaa64;
  color: white;
}

.cell.yellow {
  background-color: #c9b458;
  border-color: #c9b458;
  color: white;
}

.cell.gray {
  background-color: #787c7e;
  border-color: #787c7e;
  color: white;
}

@keyframes flip {
  0% {
    transform: scaleY(1);
  }
  50% {
    transform: scaleY(0);
  }
  100% {
    transform: scaleY(1);
  }
}
</style> 