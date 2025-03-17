<template>
  <div :dir="dir" :class="{ 'dark': themeStore.isDark }">
    <MainNav />
    <main>
      <div class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <Toast />
        <slot />
      </div>
    </main>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useTranslations } from '@/composables/useTranslations'
import Toast from '~/components/Notification/Toast.vue'
import MainNav from '~/components/Navigation/MainNav.vue'
import { useAuthStore } from '~/stores/auth'
import { useThemeStore } from '~/stores/theme'

const { dir } = useTranslations()
const authStore = useAuthStore()
const themeStore = useThemeStore()

onMounted(() => {
  if (process.client) {
    authStore.initializeAuth()
    themeStore.initTheme()
  }
})
</script> 