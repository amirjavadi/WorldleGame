<template>
  <div :dir="dir" :class="{ 'dark': themeStore.isDark }">
    <nav class="bg-white dark:bg-gray-800 shadow">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex">
            <div class="flex-shrink-0 flex items-center">
              <NuxtLink to="/" class="text-xl font-bold text-gray-900 dark:text-white">
                وردل
              </NuxtLink>
            </div>
            <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
              <NuxtLink
                to="/"
                class="border-transparent text-gray-500 dark:text-gray-300 hover:border-gray-300 dark:hover:border-gray-600 hover:text-gray-700 dark:hover:text-gray-200 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
                active-class="border-indigo-500 text-gray-900 dark:text-white"
              >
                بازی
              </NuxtLink>
              <template v-if="authStore.isAuthenticated">
                <NuxtLink
                  to="/daily-challenge"
                  class="border-transparent text-gray-500 dark:text-gray-300 hover:border-gray-300 dark:hover:border-gray-600 hover:text-gray-700 dark:hover:text-gray-200 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
                  active-class="border-indigo-500 text-gray-900 dark:text-white"
                >
                  چالش روزانه
                </NuxtLink>
                <NuxtLink
                  to="/leaderboard"
                  class="border-transparent text-gray-500 dark:text-gray-300 hover:border-gray-300 dark:hover:border-gray-600 hover:text-gray-700 dark:hover:text-gray-200 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
                  active-class="border-indigo-500 text-gray-900 dark:text-white"
                >
                  جدول رتبه‌بندی
                </NuxtLink>
                <NuxtLink
                  to="/stats"
                  class="border-transparent text-gray-500 dark:text-gray-300 hover:border-gray-300 dark:hover:border-gray-600 hover:text-gray-700 dark:hover:text-gray-200 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
                  active-class="border-indigo-500 text-gray-900 dark:text-white"
                >
                  آمار
                </NuxtLink>
              </template>
            </div>
          </div>
          <div class="hidden sm:ml-6 sm:flex sm:items-center">
            <button
              @click="themeStore.toggleTheme"
              class="p-2 text-gray-500 dark:text-gray-300 hover:text-gray-700 dark:hover:text-gray-200 rounded-md"
            >
              <span v-if="themeStore.isDark">☀️</span>
              <span v-else>🌙</span>
            </button>
            <div v-if="authStore.isAuthenticated" class="flex items-center space-x-4">
              <span class="text-gray-700 dark:text-gray-300">{{ authStore.username }}</span>
              <button
                @click="authStore.logout"
                class="text-gray-500 dark:text-gray-300 hover:text-gray-700 dark:hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium"
              >
                خروج
              </button>
            </div>
            <template v-else>
              <NuxtLink
                to="/login"
                class="text-gray-500 dark:text-gray-300 hover:text-gray-700 dark:hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium"
              >
                ورود
              </NuxtLink>
              <NuxtLink
                to="/register"
                class="text-gray-500 dark:text-gray-300 hover:text-gray-700 dark:hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium"
              >
                ثبت‌نام
              </NuxtLink>
            </template>
          </div>
        </div>
      </div>
    </nav>

    <main>
      <div class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <Toast />
        <slot />
      </div>
    </main>
  </div>
</template>

<script setup>
import { useTranslations } from '@/composables/useTranslations'
import Toast from '~/components/Notification/Toast.vue'
import { useAuthStore } from '~/stores/auth'
import { useThemeStore } from '~/stores/theme'

const { dir } = useTranslations()
const authStore = useAuthStore()
const themeStore = useThemeStore()

// Initialize auth and theme state when component mounts
onMounted(() => {
  authStore.initializeAuth()
  themeStore.initTheme()
})
</script> 