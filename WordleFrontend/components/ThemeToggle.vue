<template>
  <button
    @click="toggleTheme"
    class="fixed top-4 left-4 p-2 rounded-full bg-gray-200 dark:bg-gray-700 hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
    :title="colorMode.value === 'dark' ? 'تغییر به حالت روشن' : 'تغییر به حالت تاریک'"
  >
    <span v-if="colorMode.value === 'dark'" class="text-yellow-500">
      <i class="fas fa-sun"></i>
    </span>
    <span v-else class="text-gray-700">
      <i class="fas fa-moon"></i>
    </span>
  </button>
</template>

<script setup>
const colorMode = useColorMode()

const toggleTheme = () => {
  colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
}
</script> 