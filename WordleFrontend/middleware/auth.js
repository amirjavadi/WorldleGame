import { useAuth } from '~/composables/useAuth'

export default defineNuxtRouteMiddleware((to) => {
  // Skip auth check on server-side
  if (process.server) return
  
  const { checkAuth } = useAuth()
  
  // Allow access to auth pages (login/register) without authentication
  if (to.path === '/login' || to.path === '/register') {
    return
  }

  // Check if user is authenticated or is a guest
  if (!checkAuth()) {
    return navigateTo('/login')
  }
}) 