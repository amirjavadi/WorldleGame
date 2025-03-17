import { useAuthStore } from '~/stores/auth'

export default defineNuxtPlugin(async () => {
  if (process.client) {
    const authStore = useAuthStore()
    
    // تاخیر کوتاه برای اطمینان از hydration کامل
    setTimeout(async () => {
      await authStore.checkAuth()
    }, 100)

    // بررسی مجدد در هنگام بازگشت به تب
    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'visible') {
        authStore.checkAuth()
      }
    })
  }
}) 