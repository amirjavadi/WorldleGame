<template>
  <div class="fixed bottom-4 left-4 bg-white dark:bg-gray-800 p-4 rounded-lg shadow-lg max-w-md text-sm">
    <h3 class="font-bold mb-2">LocalStorage Contents:</h3>
    <pre class="whitespace-pre-wrap overflow-auto max-h-48">{{ storageContents }}</pre>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const storageContents = ref({})

const updateStorageContents = () => {
  if (process.client) {
    const contents = {}
    for (let i = 0; i < localStorage.length; i++) {
      const key = localStorage.key(i)
      try {
        contents[key] = JSON.parse(localStorage.getItem(key))
      } catch {
        contents[key] = localStorage.getItem(key)
      }
    }
    storageContents.value = contents
  }
}

onMounted(() => {
  updateStorageContents()
  // Listen for storage changes
  window.addEventListener('storage', updateStorageContents)
})
</script> 