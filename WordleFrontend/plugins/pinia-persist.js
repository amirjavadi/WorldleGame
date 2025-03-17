import { defineNuxtPlugin } from '#app'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'
import { useAuthStore } from '~/stores/auth'

export default defineNuxtPlugin((nuxtApp) => {
  // فقط افزودن پلاگین persistedstate به Pinia
  nuxtApp.$pinia.use(piniaPluginPersistedstate)
  
  // اطمینان از اینکه فقط در سمت کلاینت اجرا می‌شود
  if (process.client) {
    const auth = useAuthStore()

    // اضافه کردن یک تایمر کوتاه برای اطمینان از hydration کامل
    setTimeout(() => {
      // بررسی وضعیت احراز هویت
      auth.checkAuth()
    }, 100)

    // اضافه کردن event listener برای visibility change
    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'visible') {
        auth.checkAuth()
      }
    })
  }
}) 