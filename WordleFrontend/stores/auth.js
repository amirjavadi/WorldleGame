import { defineStore } from 'pinia'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

export const useAuthStore = defineStore('auth', () => {
  const router = useRouter()
  const isAuthenticated = ref(false)
  const username = ref('')
  const isGuest = ref(false)

  // تابع برای بررسی وضعیت احراز هویت
  const checkAuth = async () => {
    try {
      const token = localStorage.getItem('token')
      const storedUsername = localStorage.getItem('username')
      const isGuestUser = localStorage.getItem('isGuest')
      
      if (token) {
        const response = await fetch('http://localhost:5275/api/auth/verify', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        })
        
        if (response.ok) {
          const data = await response.json()
          isAuthenticated.value = true
          username.value = storedUsername || data.username
          localStorage.setItem('username', username.value)
          isGuest.value = false
        } else {
          await logout()
        }
      } else if (isGuestUser === 'true') {
        isGuest.value = true
        username.value = 'مهمان'
        isAuthenticated.value = false
        // مهمان نباید به صفحه لاگین هدایت شود
      } else {
        isGuest.value = false
        isAuthenticated.value = false
        username.value = ''
        router.push('/login')
      }
    } catch (error) {
      console.error('Error checking auth:', error)
      await logout()
    }
  }

  // تابع لاگین
  const login = async (credentials) => {
    try {
      const response = await fetch('http://localhost:5275/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(credentials)
      })

      if (response.ok) {
        const data = await response.json()
        localStorage.setItem('token', data.token)
        localStorage.setItem('username', data.username)
        localStorage.removeItem('isGuest') // پاک کردن وضعیت مهمان در صورت لاگین
        isAuthenticated.value = true
        username.value = data.username
        isGuest.value = false
        router.push('/')
        return { success: true }
      } else {
        const error = await response.json()
        return { success: false, error: error.message }
      }
    } catch (error) {
      console.error('Login error:', error)
      return { success: false, error: 'خطا در ارتباط با سرور' }
    }
  }

  // تابع لاگ‌اوت
  const logout = async () => {
    localStorage.removeItem('token')
    localStorage.removeItem('username')
    localStorage.removeItem('isGuest')
    isAuthenticated.value = false
    username.value = ''
    isGuest.value = false
    router.push('/login')
  }

  // تابع ورود به عنوان مهمان
  const loginAsGuest = () => {
    isGuest.value = true
    username.value = 'مهمان'
    isAuthenticated.value = false
    localStorage.setItem('isGuest', 'true')
    localStorage.removeItem('token') // اطمینان از پاک شدن توکن قبلی
    localStorage.removeItem('username') // اطمینان از پاک شدن نام کاربری قبلی
    router.push('/')
  }

  // بررسی اولیه وضعیت احراز هویت
  if (process.client) {
    checkAuth()
  }

  return {
    isAuthenticated,
    username,
    isGuest,
    login,
    logout,
    loginAsGuest,
    checkAuth
  }
}, {
  persist: {
    storage: localStorage,
    paths: ['isAuthenticated', 'username', 'isGuest']
  }
}) 