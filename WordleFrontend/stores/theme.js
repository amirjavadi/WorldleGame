import { defineStore } from 'pinia'
import { ref, watch } from 'vue'
import { useCookie } from '#imports'

export const useThemeStore = defineStore('theme', () => {
  const isDark = ref(false)
  const themeCookie = useCookie('theme')

  // بررسی تم ذخیره شده
  const initTheme = () => {
    const savedTheme = themeCookie.value
    if (savedTheme) {
      isDark.value = savedTheme === 'dark'
    } else if (process.client) {
      // بررسی تنظیمات سیستم
      isDark.value = window.matchMedia('(prefers-color-scheme: dark)').matches
    }
    applyTheme()
  }

  // اعمال تم به document
  const applyTheme = () => {
    if (process.client) {
      if (isDark.value) {
        document.documentElement.classList.add('dark')
      } else {
        document.documentElement.classList.remove('dark')
      }
      themeCookie.value = isDark.value ? 'dark' : 'light'
    }
  }

  // تغییر تم
  const toggleTheme = () => {
    isDark.value = !isDark.value
  }

  // نظارت بر تغییرات تم
  watch(isDark, () => {
    applyTheme()
  })

  // اجرای initTheme در زمان ایجاد store
  initTheme()

  return {
    isDark,
    toggleTheme,
    initTheme
  }
}) 