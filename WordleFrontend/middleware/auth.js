import { useAuthStore } from '~/stores/auth'
import { useNotification } from '~/composables/useNotification'

export default defineNuxtRouteMiddleware(async (to) => {
  const authStore = useAuthStore()
  const { addNotification } = useNotification()
  const publicPages = ['/login', '/register']
  const authRequired = !publicPages.includes(to.path)

  if (!authStore.isAuthenticated && authRequired) {
    addNotification('لطفاً ابتدا وارد شوید', 'info')
    return navigateTo('/login')
  }

  if (authStore.isAuthenticated && publicPages.includes(to.path)) {
    return navigateTo('/')
  }
}) 