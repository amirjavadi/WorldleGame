import { useAuthStore } from '~/stores/auth'
import { useNotification } from '~/composables/useNotification'

export default defineNuxtRouteMiddleware(async (to) => {
  const authStore = useAuthStore()
  const { addNotification } = useNotification()
  
  // صفحاتی که نیاز به احراز هویت ندارند
  const publicPages = ['/login', '/register']
  const isPublicPage = publicPages.includes(to.path)

  // بررسی وضعیت احراز هویت
  if (!isPublicPage) {
    const isAuthenticated = await authStore.checkAuth()
    
    if (!isAuthenticated && !authStore.isGuestUser) {
      return navigateTo('/login')
    }
  }

  // اگر کاربر مهمان است، اجازه دسترسی به همه صفحات به جز login/register را دارد
  if (authStore.isGuest && publicPages.includes(to.path)) {
    return navigateTo('/')
  }

  // اگر کاربر لاگین است و می‌خواهد به صفحات عمومی برود
  if (authStore.isAuthenticated && publicPages.includes(to.path)) {
    return navigateTo('/')
  }
}) 