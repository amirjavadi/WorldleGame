<template>
  <NuxtPage />
</template>

<script setup>
import { onMounted } from 'vue'
import { useTranslations } from './composables/useTranslations'

const { locale } = useTranslations()

onMounted(() => {
  // Set initial theme
  if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
    document.documentElement.classList.add('dark')
  }
  // Set RTL direction
  document.documentElement.dir = 'rtl'
})
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;500;600;700&display=swap');

html {
  font-family: 'Vazirmatn', system-ui, sans-serif;
  direction: rtl;
}

body {
  @apply bg-gray-50 dark:bg-gray-900 text-gray-900 dark:text-white;
}

.dark {
  color-scheme: dark;
}

/* RTL specific styles */
.space-x-2 {
  @apply space-x-reverse;
}

.space-x-4 {
  @apply space-x-reverse;
}

.space-x-8 {
  @apply space-x-reverse;
}

.pl-3 {
  @apply pr-3;
}

.left-0 {
  @apply right-0;
}

.rounded-t-md {
  @apply rounded-tr-md rounded-tl-md;
}

.rounded-b-md {
  @apply rounded-br-md rounded-bl-md;
}
</style>
