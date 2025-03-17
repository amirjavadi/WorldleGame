<template>
  <div>
    <ColorScheme>
      <ThemeToggle />
      <Notifications />
      <NuxtPage />
      <GuestModal v-if="auth.shouldShowGuestModal" @close="auth.hideGuestModal" />
    </ColorScheme>
  </div>
</template>

<script setup>
import { onBeforeMount } from 'vue'
import { useAuthStore } from '~/stores/auth'

const auth = useAuthStore()

onBeforeMount(() => {
  if (process.client) {
    const locale = localStorage.getItem('preferredLocale') || 'fa'
    document.documentElement.dir = locale === 'fa' ? 'rtl' : 'ltr'
    auth.checkAuth()
  }
})
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;500;600;700&display=swap');

html {
  font-family: 'Vazirmatn', system-ui, sans-serif;
}

body {
  @apply bg-gray-50 dark:bg-gray-900 text-gray-900 dark:text-white transition-colors duration-200;
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
